import { useCallback, useEffect, useState } from 'react';
import { deleteFile, downloadFile, downloadProjectZip, getFileBlobUrl, getFiles, openInBrowser, uploadFile } from '../services/fileService';

export function useFiles(projectId) {
  const [files, setFiles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const refresh = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getFiles(projectId);
      setFiles(data);
    } catch (err) {
      setError(err.message || 'Failed to load files');
    } finally {
      setLoading(false);
    }
  }, [projectId]);

  useEffect(() => {
    if (projectId) {
      refresh();
    }
  }, [projectId, refresh]);

  const upload = useCallback(async (file) => {
    const created = await uploadFile(projectId, file);
    setFiles((prev) => [...prev, created]);
    return created;
  }, [projectId]);

  const remove = useCallback(async (id) => {
    await deleteFile(id);
    setFiles((prev) => prev.filter((f) => f.id !== id));
  }, []);

  const download = useCallback(async (id) => {
    await downloadFile(id);
  }, []);

  const downloadZip = useCallback(async () => {
    await downloadProjectZip(projectId);
  }, [projectId]);

  const previewUrl = useCallback(async (id) => {
    return getFileBlobUrl(id);
  }, []);

  const openFileInBrowser = useCallback(async (id) => {
    await openInBrowser(id);
  }, []);

  return {
    files,
    loading,
    error,
    refresh,
    upload,
    remove,
    download,
    downloadZip,
    previewUrl,
    openFileInBrowser
  };
}
