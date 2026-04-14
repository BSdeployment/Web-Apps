import { Box, Typography } from '@mui/material';
import MDEditor from '@uiw/react-md-editor';
import '@uiw/react-md-editor/markdown-editor.css';


function MarkdownEditor({ initialMarkdown, onChange }) {
  const handleChange = (value) => {
    const markdown = value ?? '';
    const wordCount = markdown.trim() ? markdown.trim().split(/\s+/).length : 0;
    onChange(markdown, wordCount);
  };

  return (
    <Box data-color-mode="light" sx={{ width: '100%' }}>
      <MDEditor value={initialMarkdown || ''} onChange={handleChange} preview="live" height={600} style={{ width: '100%' }} />
      <Typography variant="caption" color="text.secondary" display="block" mt={1}>
        Tip: Use headings and sections to keep your draft structured.
      </Typography>
    </Box>
  );
}

export default MarkdownEditor;
