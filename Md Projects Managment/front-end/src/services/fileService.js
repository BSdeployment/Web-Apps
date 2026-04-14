import { apiFetch, buildUrl } from './api';

export function getFiles(projectId) {
  return apiFetch(`/api/projects/${projectId}/files`);
}

export function uploadFile(projectId, file) {
  const formData = new FormData();
  formData.append('file', file);
  return apiFetch(`/api/projects/${projectId}/files`, {
    method: 'POST',
    body: formData
  });
}

export function deleteFile(fileId) {
  return apiFetch(`/api/files/${fileId}`, { method: 'DELETE' });
}

export async function downloadFile(fileId) {
  const response = await fetch(buildUrl(`/api/files/${fileId}/download`));
  if (!response.ok) {
    throw new Error('Failed to download file');
  }

  const blob = await response.blob();
  const contentDisposition = response.headers.get('content-disposition') || '';
  const match = /filename="?([^";]+)"?/i.exec(contentDisposition);
  const fileName = match?.[1] || `file_${fileId}`;
  triggerBrowserDownload(blob, fileName);
}

export async function downloadProjectZip(projectId) {
  const response = await fetch(buildUrl(`/api/projects/${projectId}/files/zip`));
  if (!response.ok) {
    throw new Error('Failed to download zip');
  }

  const blob = await response.blob();
  const contentDisposition = response.headers.get('content-disposition') || '';
  const match = /filename="?([^";]+)"?/i.exec(contentDisposition);
  const fileName = match?.[1] || `project_${projectId}_files.zip`;
  triggerBrowserDownload(blob, fileName);
}

export async function getFileBlobUrl(fileId) {
  const response = await fetch(buildUrl(`/api/files/${fileId}/download`));
  if (!response.ok) {
    throw new Error('Failed to load file');
  }
  const blob = await response.blob();
  return URL.createObjectURL(blob);
}

export async function openInBrowser(fileId) {
  const url = await getFileBlobUrl(fileId);
  window.open(url, '_blank', 'noopener,noreferrer');
  setTimeout(() => URL.revokeObjectURL(url), 5000);
}

function triggerBrowserDownload(blob, fileName) {
  const url = URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.href = url;
  link.download = fileName;
  document.body.appendChild(link);
  link.click();
  link.remove();
  URL.revokeObjectURL(url);
}
