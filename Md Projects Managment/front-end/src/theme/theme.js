import { createTheme } from '@mui/material/styles';

const theme = createTheme({
  palette: {
    mode: 'light',
    primary: {
      main: '#1f4b5c'
    },
    secondary: {
      main: '#b55b2a'
    },
    background: {
      default: '#f6f4f1',
      paper: '#ffffff'
    }
  },
  typography: {
    fontFamily: '"IBM Plex Sans", "Segoe UI", Tahoma, sans-serif',
    h4: {
      fontWeight: 600
    },
    h5: {
      fontWeight: 600
    }
  },
  shape: {
    borderRadius: 12
  }
});

export default theme;
