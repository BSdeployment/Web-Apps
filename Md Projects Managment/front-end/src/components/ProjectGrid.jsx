import { Grid } from '@mui/material';
import ProjectCard from './ProjectCard';

function ProjectGrid({ projects, onOpen, onDelete }) {
  return (
    <Grid container spacing={3}>
      {projects.map((project) => (
        <Grid key={project.id} item xs={12} sm={6} md={4}>
          <ProjectCard project={project} onOpen={onOpen} onDelete={onDelete} />
        </Grid>
      ))}
    </Grid>
  );
}

export default ProjectGrid;
