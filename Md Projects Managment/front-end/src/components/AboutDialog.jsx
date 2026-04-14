import {
  Dialog, DialogContent, DialogTitle, IconButton,
  Button, Avatar, Box, Typography, Chip, Divider, Link, Stack
} from '@mui/material';
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';
import GitHubIcon from '@mui/icons-material/GitHub';
import ArticleIcon from '@mui/icons-material/Article';
import EmailIcon from '@mui/icons-material/Email';
import CloseIcon from '@mui/icons-material/Close';
import { useState } from 'react';

export default function AboutDialog() {
  const [open, setOpen] = useState(false);

  return (
    <>
      <Button
      style={{margin:"20px 0px 0px"}}
        variant="outlined"
        size="small"
        startIcon={<InfoOutlinedIcon />}
        onClick={() => setOpen(true)}
        sx={{ textTransform: 'none', borderRadius: 2 }}
      >
        אודות
      </Button>

      <Dialog dir='rtl' open={open} onClose={() => setOpen(false)} maxWidth="xs" fullWidth>
        <DialogTitle sx={{ pb: 1 }}>
          <Box display="flex" justifyContent="space-between" alignItems="flex-start">
            <Box display="flex" alignItems="center" gap={1.5}>
              <Avatar sx={{ bgcolor: '#EEEDFE', color: '#3C3489', fontWeight: 500 }}>
                BS
              </Avatar>
              <Box>
                <Typography fontWeight={500}>Bs DotNet</Typography>
                <Typography variant="caption" color="text.secondary">מפתח עצמאי</Typography>
              </Box>
            </Box>
            <IconButton size="small" onClick={() => setOpen(false)}>
              <CloseIcon fontSize="small" />
            </IconButton>
          </Box>
        </DialogTitle>

        <DialogContent>
          <Stack direction="row" flexWrap="wrap" gap={1} mb={2}>
            <Chip
              icon={<GitHubIcon />}
              label="GitHub"
              size="small"
              component={Link}
              href="https://github.com/BSdeployment/"
              target="_blank"
              clickable
            />
            <Chip
              icon={<ArticleIcon />}
              label="אתר מאמרים"
              size="small"
              component={Link}
              href="https://bsdeployment.github.io/"
              target="_blank"
              clickable
            />
            <Chip
              icon={<EmailIcon />}
              label="מייל"
              size="small"
              component={Link}
              href="mailto:w0583253532@gmail.com"
              clickable
            />
          </Stack>

          <Divider sx={{ mb: 2 }} />

          <Typography variant="body2" color="text.secondary" lineHeight={1.7} mb={2}>
            אפליקציה ליצירת פרויקטים ומאמרים בפורמט Markdown — כולל עורך ויזואלי,
            מארגן קישורים ותמונות, וייצוא קובץ MD.
          </Typography>

          <Stack direction="row" flexWrap="wrap" gap={0.75} mb={2}>
            {['ASP.NET Core', 'React', 'SQLite', 'רץ מקומית'].map(tag => (
              <Chip key={tag} label={tag} size="small" variant="outlined" />
            ))}
          </Stack>

          <Divider sx={{ mb: 1.5 }} />

          <Box display="flex" justifyContent="space-between" alignItems="center">
            <Typography variant="caption" color="text.secondary">רישיון חופשי ופתוח</Typography>
            <Link
              href="https://github.com/BSdeployment/"
              target="_blank"
              variant="caption"
              underline="hover"
            >
              קוד מקור — web apps ↗
            </Link>
          </Box>
        </DialogContent>
      </Dialog>
    </>
  );
}