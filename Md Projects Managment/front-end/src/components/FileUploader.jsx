import { Box, Button, Typography } from '@mui/material';
import CloudUploadIcon from '@mui/icons-material/CloudUpload';
import { useRef, useState } from 'react';

function FileUploader({ onUpload }) {
  const inputRef = useRef(null);
  const [dragging, setDragging] = useState(false);

  const handleFiles = (files) => {
    if (!files || files.length === 0) {
      return;
    }
    onUpload(files[0]);
  };

  return (
    <Box
      onDragOver={(event) => {
        event.preventDefault();
        setDragging(true);
      }}
      onDragLeave={() => setDragging(false)}
      onDrop={(event) => {
        event.preventDefault();
        setDragging(false);
        handleFiles(event.dataTransfer.files);
      }}
      sx={{
        border: '1px dashed',
        borderColor: dragging ? 'primary.main' : 'divider',
        borderRadius: 2,
        p: 3,
        textAlign: 'center',
        bgcolor: dragging ? 'rgba(31, 75, 92, 0.04)' : 'transparent'
      }}
    >
      <Typography variant="subtitle1" gutterBottom>
        Drag and drop a file here
      </Typography>
      <Typography variant="body2" color="text.secondary" gutterBottom>
        or
      </Typography>
      <Button variant="outlined" startIcon={<CloudUploadIcon />} onClick={() => inputRef.current?.click()}>
        Upload file
      </Button>
      <input
        ref={inputRef}
        type="file"
        hidden
        onChange={(event) => handleFiles(event.target.files)}
      />
    </Box>
  );
}

export default FileUploader;
