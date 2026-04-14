import { Box, CircularProgress, Typography } from '@mui/material';

function LoadingIndicator({ label = 'Loading...' }) {
  return (
    <Box display="flex" alignItems="center" gap={2} py={2}>
      <CircularProgress size={22} />
      <Typography variant="body2" color="text.secondary">
        {label}
      </Typography>
    </Box>
  );
}

export default LoadingIndicator;
