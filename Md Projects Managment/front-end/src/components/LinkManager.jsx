import { Box, Button, IconButton, Stack, TextField, Typography } from '@mui/material';
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import { useState } from 'react';

function LinkManager({ links, onAdd, onDelete }) {
  const [title, setTitle] = useState('');
  const [url, setUrl] = useState('');
  const [notes, setNotes] = useState('');

  const handleSubmit = async (event) => {
    event.preventDefault();
    if (!url.trim()) {
      return;
    }
    await onAdd({ title, url, notes });
    setTitle('');
    setUrl('');
    setNotes('');
  };

  return (
    <Box>
      <Typography variant="h6" gutterBottom>
        Links
      </Typography>
      <Stack component="form" spacing={2} onSubmit={handleSubmit} sx={{ mb: 3 }}>
        <TextField label="Title" value={title} onChange={(e) => setTitle(e.target.value)} />
        <TextField label="URL" required value={url} onChange={(e) => setUrl(e.target.value)} />
        <TextField label="Notes" value={notes} onChange={(e) => setNotes(e.target.value)} multiline minRows={2} />
        <Button type="submit" variant="contained">
          Add link
        </Button>
      </Stack>

      <Stack spacing={2}>
        {links.map((link) => (
          <Box key={link.id} sx={{ p: 2, border: '1px solid', borderColor: 'divider', borderRadius: 2 }}>
            <Stack direction="row" justifyContent="space-between" alignItems="center">
              <Box>
                <Typography variant="subtitle1">{link.title || link.url}</Typography>
                <Typography variant="body2" color="text.secondary">
                  {link.url}
                </Typography>
              </Box>
              <IconButton onClick={() => onDelete(link.id)} aria-label="Delete link">
                <DeleteOutlineIcon fontSize="small" />
              </IconButton>
            </Stack>
            {link.notes && (
              <Typography variant="body2" sx={{ mt: 1 }}>
                {link.notes}
              </Typography>
            )}
          </Box>
        ))}
        {links.length === 0 && (
          <Typography variant="body2" color="text.secondary">
            No links yet.
          </Typography>
        )}
      </Stack>
    </Box>
  );
}

export default LinkManager;
