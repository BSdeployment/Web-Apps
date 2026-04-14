import { Card, CardActions, CardContent, Chip, IconButton, Stack, Typography } from '@mui/material';
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import OpenInNewIcon from '@mui/icons-material/OpenInNew';
import { formatDate } from '../utils/date';

function ProjectCard({ project, onOpen, onDelete }) {
  return (
    <Card variant="outlined" sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
      <CardContent sx={{ flexGrow: 1 }}>
        <Stack direction="row" alignItems="center" justifyContent="space-between" mb={1}>
          <Typography variant="h6" component="h3">
            {project.title}
          </Typography>
          <Chip
            size="small"
            label={project.completed ? 'Completed' : 'In progress'}
            color={project.completed ? 'secondary' : 'default'}
            variant={project.completed ? 'filled' : 'outlined'}
          />
        </Stack>
        <Typography variant="body2" color="text.secondary" sx={{ minHeight: 42 }}>
          {project.description || 'No description yet.'}
        </Typography>
        <Typography variant="caption" color="text.secondary" display="block" mt={2}>
          Created {formatDate(project.createdAt)}
        </Typography>
      </CardContent>
      <CardActions sx={{ justifyContent: 'space-between', px: 2, pb: 2 }}>
        <IconButton size="small" onClick={() => onDelete(project)} aria-label="Delete project">
          <DeleteOutlineIcon fontSize="small" />
        </IconButton>
        <IconButton size="small" onClick={() => onOpen(project)} aria-label="Open project">
          <OpenInNewIcon fontSize="small" />
        </IconButton>
      </CardActions>
    </Card>
  );
}

export default ProjectCard;
