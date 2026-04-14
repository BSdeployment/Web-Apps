import { useMemo, useState } from 'react';
import { Alert, Box, Button, Container, Dialog, DialogActions, DialogContent, DialogTitle, Stack, TextField, Typography } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import  Info  from '@mui/icons-material/Info';
import WarningAmberIcon from '@mui/icons-material/WarningAmber';
import { useNavigate } from 'react-router-dom';
import ProjectGrid from '../components/ProjectGrid';
import ProjectSearchBar from '../components/ProjectSearchBar';
import LoadingIndicator from '../components/LoadingIndicator';
import { useProjects } from '../hooks/useProjects';
import AboutDialog from '../components/AboutDialog';
function DashboardPage() {
  const navigate = useNavigate();
  const { projects, loading, error, create, remove } = useProjects();
  const [search, setSearch] = useState('');
  const [dialogOpen, setDialogOpen] = useState(false);
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [deleteTarget, setDeleteTarget] = useState(null);

  const filteredProjects = useMemo(() => {
    const query = search.trim().toLowerCase();
    if (!query) {
      return projects;
    }
    return projects.filter((project) => project.title.toLowerCase().includes(query));
  }, [projects, search]);

  const handleCreate = async () => {
    const payload = {
      title: title.trim(),
      description: description.trim(),
      completed: false
    };
    const created = await create(payload);
    setDialogOpen(false);
    setTitle('');
    setDescription('');
    navigate(`/projects/${created.id}`);
  };

  const confirmDelete = async () => {
    if (!deleteTarget) {
      return;
    }
    await remove(deleteTarget.id);
    setDeleteTarget(null);
  };

  return (
    <Box sx={{ py: 4 }}>
      <Container maxWidth="lg">
        <Stack direction={{ xs: 'column', md: 'row' }} spacing={2} alignItems={{ xs: 'stretch', md: 'center' }} mb={4}>
          <Box flexGrow={1}>
            <Typography variant="h4" gutterBottom>
              Writing Projects
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Track research, draft articles, and manage files in one workspace.
            </Typography>
            <AboutDialog />
          </Box>
          <Button variant="contained" startIcon={<AddIcon />} onClick={() => setDialogOpen(true)}>
            New Project
          </Button>
        </Stack>

        <Box mb={3}>
          <ProjectSearchBar value={search} onChange={setSearch} />
        </Box>

        {loading && <LoadingIndicator label="Loading projects" />}
        {error && <Alert severity="error">{error}</Alert>}
        {!loading && filteredProjects.length === 0 && (
          <Typography variant="body2" color="text.secondary">
            No projects found.
          </Typography>
        )}
        {!loading && filteredProjects.length > 0 && (
          <ProjectGrid
            projects={filteredProjects}
            onOpen={(project) => navigate(`/projects/${project.id}`)}
            onDelete={(project) => setDeleteTarget(project)}
          />
        )}
      </Container>

      <Dialog open={dialogOpen} onClose={() => setDialogOpen(false)} maxWidth="sm" fullWidth>
        <DialogTitle>Create a new project</DialogTitle>
        <DialogContent sx={{ pt: 2 }}>
          <Stack spacing={2}>
            <TextField label="Title" value={title} onChange={(e) => setTitle(e.target.value)} required />
            <TextField label="Description" value={description} onChange={(e) => setDescription(e.target.value)} multiline minRows={3} />
          </Stack>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDialogOpen(false)}>Cancel</Button>
          <Button variant="contained" onClick={handleCreate} disabled={!title.trim()}>
            Create
          </Button>
        </DialogActions>
      </Dialog>

      <Dialog open={Boolean(deleteTarget)} onClose={() => setDeleteTarget(null)} maxWidth="xs" fullWidth>
        <DialogTitle sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <WarningAmberIcon color="warning" />
          Confirm deletion
        </DialogTitle>
        <DialogContent>
          <Typography variant="body2" color="text.secondary">
            Are you sure you want to delete "{deleteTarget?.title}"? This action cannot be undone.
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteTarget(null)}>Cancel</Button>
          <Button variant="contained" color="error" onClick={confirmDelete}>
            Delete project
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}

export default DashboardPage;
