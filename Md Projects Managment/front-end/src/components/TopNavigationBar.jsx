import { AppBar, Button, Stack, Toolbar, Typography } from '@mui/material';
import SaveIcon from '@mui/icons-material/Save';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import DownloadIcon from '@mui/icons-material/Download';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import RadioButtonUncheckedIcon from '@mui/icons-material/RadioButtonUnchecked';

function TopNavigationBar({ title, saving, lastSaved, completed, onToggleCompleted, onSave, onExport, onBack }) {
  const statusText = saving ? 'Saving...' : lastSaved ? `Saved ${lastSaved}` : 'Not saved';
  const completionLabel = completed ? 'Mark In Progress' : 'Mark Complete';

  return (
    <AppBar position="sticky" color="default" elevation={0} sx={{ borderBottom: '1px solid', borderColor: 'divider' }}>
      <Toolbar sx={{ gap: 2 }}>
        <Button startIcon={<ArrowBackIcon />} onClick={onBack} variant="text">
          Dashboard
        </Button>
        <Typography variant="h6" sx={{ flexGrow: 1 }}>
          {title || 'Untitled Project'}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          {statusText}
        </Typography>
        <Stack direction="row" spacing={1}>
          <Button
            startIcon={completed ? <RadioButtonUncheckedIcon /> : <CheckCircleIcon />}
            variant="outlined"
            onClick={onToggleCompleted}
          >
            {completionLabel}
          </Button>
          <Button startIcon={<SaveIcon />} variant="contained" onClick={onSave}>
            Save Article
          </Button>
          <Button startIcon={<DownloadIcon />} variant="outlined" onClick={onExport}>
            Export
          </Button>
        </Stack>
      </Toolbar>
    </AppBar>
  );
}

export default TopNavigationBar;
