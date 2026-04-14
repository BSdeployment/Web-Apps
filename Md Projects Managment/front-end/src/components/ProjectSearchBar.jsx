import { InputAdornment, TextField } from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';

function ProjectSearchBar({ value, onChange }) {
  return (
    <TextField
      fullWidth
      placeholder="Search projects by title"
      value={value}
      onChange={(event) => onChange(event.target.value)}
      InputProps={{
        startAdornment: (
          <InputAdornment position="start">
            <SearchIcon fontSize="small" />
          </InputAdornment>
        )
      }}
    />
  );
}

export default ProjectSearchBar;
