import { Box, Button, Container, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';

function NotFoundPage() {
  const navigate = useNavigate();

  return (
    <Container maxWidth="sm" sx={{ py: 10, textAlign: 'center' }}>
      <Box>
        <Typography variant="h4" gutterBottom>
          Page not found
        </Typography>
        <Typography variant="body2" color="text.secondary" gutterBottom>
          The page you are looking for does not exist.
        </Typography>
        <Button variant="contained" onClick={() => navigate('/')}
        >
          Back to dashboard
        </Button>
      </Box>
    </Container>
  );
}

export default NotFoundPage;
