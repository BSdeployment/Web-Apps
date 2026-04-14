import { useEffect, useRef, useState } from 'react';
import { Alert, Box, Button, Container, Divider, Grid, Paper, Stack, TextField, Typography } from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';
import TopNavigationBar from '../components/TopNavigationBar';
import ExplorerPanel from '../components/ExplorerPanel';
import MarkdownEditor from '../components/MarkdownEditor';
import FileUploader from '../components/FileUploader';
import LinkManager from '../components/LinkManager';
import LoadingIndicator from '../components/LoadingIndicator';
import { useProject } from '../hooks/useProject';
import { useFiles } from '../hooks/useFiles';
import { useLinks } from '../hooks/useLinks';
import { useNotes } from '../hooks/useNotes';
import { sendArticleBeacon } from '../services/projectService';
import { sendNoteBeacon } from '../services/noteService';

function ProjectWorkspacePage() {
  const navigate = useNavigate();
  const { id } = useParams();
  const projectId = Number(id);
  const { project, article, loading, saving, error, saveArticle, toggleCompleted } = useProject(projectId);
  const { files, upload, remove, download, downloadZip, previewUrl, openFileInBrowser } = useFiles(projectId);
  const { links, add, remove: removeLink } = useLinks(projectId);
  const { note, saving: noteSaving, save: saveNote } = useNotes(projectId);

  const [selectedItem, setSelectedItem] = useState({ type: 'article' });
  const [editorContent, setEditorContent] = useState('');
  const [wordCount, setWordCount] = useState(0);
  const [lastSaved, setLastSaved] = useState('');
  const [noteContent, setNoteContent] = useState('');
  const [noteSaved, setNoteSaved] = useState('');
  const [previewImageUrl, setPreviewImageUrl] = useState('');

  const articleDirtyRef = useRef(false);
  const noteDirtyRef = useRef(false);
  const latestArticleRef = useRef('');
  const latestNoteRef = useRef('');

  const selectedFile = files.find((file) => file.id === selectedItem?.id);
  const selectedLink = links.find((link) => link.id === selectedItem?.id);

  useEffect(() => {
    if (article?.markdownContent !== undefined) {
      setEditorContent(article.markdownContent);
      latestArticleRef.current = article.markdownContent;
      articleDirtyRef.current = false;
    }
  }, [article]);

  useEffect(() => {
    if (note?.content !== undefined) {
      setNoteContent(note?.content || '');
      latestNoteRef.current = note?.content || '';
      noteDirtyRef.current = false;
    }
  }, [note]);

  useEffect(() => {
    let isActive = true;

    const loadPreview = async () => {
      if (!selectedFile || !selectedFile.fileType?.startsWith('image/')) {
        setPreviewImageUrl('');
        return;
      }
      const url = await previewUrl(selectedFile.id);
      if (isActive) {
        setPreviewImageUrl(url);
      }
    };

    loadPreview();
    return () => {
      isActive = false;
    };
  }, [selectedFile, previewUrl]);

  useEffect(() => {
    return () => {
      if (previewImageUrl) {
        URL.revokeObjectURL(previewImageUrl);
      }
    };
  }, [previewImageUrl]);




  useEffect(() => {
    const handleBeforeUnload = () => {
      if (articleDirtyRef.current) {
        sendArticleBeacon(projectId, latestArticleRef.current);
      }
      if (noteDirtyRef.current) {
        sendNoteBeacon(projectId, latestNoteRef.current);
      }
    };

    window.addEventListener('beforeunload', handleBeforeUnload);
    return () => window.removeEventListener('beforeunload', handleBeforeUnload);
  }, [projectId]);

  const handleSave = async () => {
    const saved = await saveArticle(editorContent);
    articleDirtyRef.current = false;
    if (saved?.updatedAt) {
      setLastSaved(new Date(saved.updatedAt).toLocaleTimeString());
    }
  };

  const handleExport = () => {
    const blob = new Blob([editorContent], { type: 'text/markdown' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `${project?.title || 'article'}.md`;
    document.body.appendChild(link);
    link.click();
    link.remove();
    URL.revokeObjectURL(url);
  };

  const handleNoteSave = async () => {
    const saved = await saveNote(noteContent);
    noteDirtyRef.current = false;
    const savedAt = saved?.updatedAt ? new Date(saved.updatedAt).toLocaleTimeString() : new Date().toLocaleTimeString();
    setNoteSaved(savedAt);
  };

  const handleBack = async () => {
    if (articleDirtyRef.current) {
      await saveArticle(latestArticleRef.current);
      articleDirtyRef.current = false;
    }
    if (noteDirtyRef.current) {
      await saveNote(latestNoteRef.current);
      noteDirtyRef.current = false;
    }
    navigate('/');
  };

  if (loading) {
    return <LoadingIndicator label="Loading project" />;
  }

  if (error) {
    return <Alert severity="error">{error}</Alert>;
  }

  return (
    <Box sx={{ minHeight: '100vh', bgcolor: 'background.default' }}>
      <TopNavigationBar
        title={project?.title}
        saving={saving || noteSaving}
        lastSaved={lastSaved}
        completed={project?.completed}
        onToggleCompleted={toggleCompleted}
        onSave={handleSave}
        onExport={handleExport}
        onBack={handleBack}
      />

      <Container maxWidth={false} sx={{ py: 3, px: { xs: 2, md: 3 } }}>
        <Grid container spacing={3}>
          <Grid item xs={12} md={3}>
            <Paper variant="outlined" sx={{ height: '100%' }}>
              <ExplorerPanel
                projectTitle={project?.title}
                files={files}
                links={links}
                selectedItem={selectedItem}
                onSelect={setSelectedItem}
                onDownloadZip={downloadZip}
              />
            </Paper>
          </Grid>
          <Grid item xs={12} md={9}>
            <Paper variant="outlined" sx={{ p: 3, minHeight: 600 }}>
              {selectedItem?.type === 'article' && (
                <Box>
                  <Stack direction="row" alignItems="center" justifyContent="space-between" mb={2}>
                    <Typography variant="h6">Article</Typography>
                    <Typography variant="body2" color="text.secondary">
                      {wordCount} words
                    </Typography>
                  </Stack>
                  <MarkdownEditor
                    initialMarkdown={editorContent}
                    onChange={(markdown, words) => {
                      setEditorContent(markdown);
                      setWordCount(words);
                      latestArticleRef.current = markdown;
                      articleDirtyRef.current = true;
                    }}
                  />
                </Box>
              )}

              {selectedItem?.type === 'files' && (
                <Box>
                  <Typography variant="h6" gutterBottom>
                    Files
                  </Typography>
                  <FileUploader onUpload={upload} />
                  <Divider sx={{ my: 3 }} />
                  <Stack spacing={2}>
                    {files.map((file) => (
                      <Stack key={file.id} direction="row" alignItems="center" justifyContent="space-between">
                        <Box>
                          <Typography variant="subtitle1">{file.fileName}</Typography>
                          <Typography variant="caption" color="text.secondary">
                            {file.fileType || 'File'}
                          </Typography>
                        </Box>
                        <Stack direction="row" spacing={1}>
                          <Button size="small" variant="outlined" onClick={() => openFileInBrowser(file.id)}>
                            Try open in browser
                          </Button>
                          <Button size="small" variant="outlined" onClick={() => download(file.id)}>
                            Download
                          </Button>
                          <Button size="small" color="error" onClick={() => remove(file.id)}>
                            Delete
                          </Button>
                        </Stack>
                      </Stack>
                    ))}
                    {files.length === 0 && (
                      <Typography variant="body2" color="text.secondary">
                        No files uploaded yet.
                      </Typography>
                    )}
                  </Stack>
                </Box>
              )}

              {selectedItem?.type === 'file' && selectedFile && (
                <Box>
                  <Typography variant="h6" gutterBottom>
                    {selectedFile.fileName}
                  </Typography>
                  <Typography variant="body2" color="text.secondary" gutterBottom>
                    {selectedFile.fileType || 'File'}
                  </Typography>
                  <Stack direction="row" spacing={2} sx={{ mb: 2 }}>
                    <Button variant="outlined" onClick={() => openFileInBrowser(selectedFile.id)}>
                      Try open in browser
                    </Button>
                    <Button variant="contained" onClick={() => download(selectedFile.id)}>
                      Download
                    </Button>
                    <Button color="error" onClick={() => remove(selectedFile.id)}>
                      Delete
                    </Button>
                  </Stack>
                  {previewImageUrl && (
                    <Box
                      component="img"
                      src={previewImageUrl}
                      alt={selectedFile.fileName}
                      sx={{ maxWidth: 360, borderRadius: 2, border: '1px solid', borderColor: 'divider' }}
                    />
                  )}
                </Box>
              )}

              {selectedItem?.type === 'links' && (
                <LinkManager links={links} onAdd={add} onDelete={removeLink} />
              )}

              {selectedItem?.type === 'link' && selectedLink && (
                <Box>
                  <Typography variant="h6" gutterBottom>
                    {selectedLink.title || selectedLink.url}
                  </Typography>
                  <Typography variant="body2" color="text.secondary" gutterBottom>
                    {selectedLink.url}
                  </Typography>
                  {selectedLink.notes && <Typography variant="body1">{selectedLink.notes}</Typography>}
                  <Button color="error" sx={{ mt: 2 }} onClick={() => removeLink(selectedLink.id)}>
                    Delete link
                  </Button>
                </Box>
              )}

              {selectedItem?.type === 'notes' && (
                <Box>
                  <Stack direction="row" alignItems="center" justifyContent="space-between" mb={2}>
                    <Typography variant="h6">Notes</Typography>
                    <Typography variant="body2" color="text.secondary">
                      {noteSaving ? 'Saving...' : noteSaved ? `Saved ${noteSaved}` : 'Not saved'}
                    </Typography>
                  </Stack>
                  <TextField
                    label="Project notes"
                    value={noteContent}
                    onChange={(event) => {
                      const value = event.target.value;
                      setNoteContent(value);
                      latestNoteRef.current = value;
                      noteDirtyRef.current = true;
                    }}
                    multiline
                    minRows={8}
                    fullWidth
                    sx={{ mb: 2 }}
                  />
                  <Button variant="contained" onClick={handleNoteSave} disabled={noteSaving}>
                    Save notes
                  </Button>
                </Box>
              )}
            </Paper>
          </Grid>
        </Grid>
      </Container>
    </Box>
  );
}

export default ProjectWorkspacePage;
