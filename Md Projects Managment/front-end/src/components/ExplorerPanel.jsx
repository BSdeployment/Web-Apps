import { Box, Button, Divider, List, ListItemButton, ListItemText, Typography } from '@mui/material';
import DownloadIcon from '@mui/icons-material/Download';

function ExplorerPanel({ projectTitle, files, links, selectedItem, onSelect, onDownloadZip }) {
  return (
    <Box sx={{ p: 2 }}>
      <Typography variant="subtitle2" color="text.secondary" sx={{ mb: 1 }}>
        PROJECT
      </Typography>
      <Typography variant="h6" sx={{ mb: 2 }}>
        {projectTitle || 'Workspace'}
      </Typography>

      <List dense>
        <Typography variant="caption" color="text.secondary">
          Article
        </Typography>
        <ListItemButton
          selected={selectedItem?.type === 'article'}
          onClick={() => onSelect({ type: 'article' })}
        >
          <ListItemText primary="Article.md" />
        </ListItemButton>

        <Divider sx={{ my: 1 }} />

        <Box display="flex" alignItems="center" justifyContent="space-between">
          <Typography variant="caption" color="text.secondary">
            Files
          </Typography>
          <Button size="small" startIcon={<DownloadIcon />} onClick={onDownloadZip}>
            Zip
          </Button>
        </Box>
        <ListItemButton
          selected={selectedItem?.type === 'files'}
          onClick={() => onSelect({ type: 'files' })}
        >
          <ListItemText primary="All Files" />
        </ListItemButton>
        {files.length === 0 && (
          <Typography variant="body2" color="text.secondary" sx={{ py: 1 }}>
            No files uploaded.
          </Typography>
        )}
        {files.map((file) => (
          <ListItemButton
            key={file.id}
            selected={selectedItem?.type === 'file' && selectedItem?.id === file.id}
            onClick={() => onSelect({ type: 'file', id: file.id })}
          >
            <ListItemText primary={file.fileName} secondary={file.fileType || 'File'} />
          </ListItemButton>
        ))}

        <Divider sx={{ my: 1 }} />

        <Typography variant="caption" color="text.secondary">
          Links
        </Typography>
        <ListItemButton
          selected={selectedItem?.type === 'links'}
          onClick={() => onSelect({ type: 'links' })}
        >
          <ListItemText primary="All Links" />
        </ListItemButton>
        {links.length === 0 && (
          <Typography variant="body2" color="text.secondary" sx={{ py: 1 }}>
            No links saved.
          </Typography>
        )}
        {links.map((link) => (
          <ListItemButton
            key={link.id}
            selected={selectedItem?.type === 'link' && selectedItem?.id === link.id}
            onClick={() => onSelect({ type: 'link', id: link.id })}
          >
            <ListItemText primary={link.title || link.url} secondary={link.url} />
          </ListItemButton>
        ))}

        <Divider sx={{ my: 1 }} />

        <Typography variant="caption" color="text.secondary">
          Notes
        </Typography>
        <ListItemButton
          selected={selectedItem?.type === 'notes'}
          onClick={() => onSelect({ type: 'notes' })}
        >
          <ListItemText primary="Notes" />
        </ListItemButton>
      </List>
    </Box>
  );
}

export default ExplorerPanel;
