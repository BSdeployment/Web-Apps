import { useCallback, useEffect, useState } from 'react';
import { createLink, deleteLink, getLinks } from '../services/linkService';

export function useLinks(projectId) {
  const [links, setLinks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const refresh = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getLinks(projectId);
      setLinks(data);
    } catch (err) {
      setError(err.message || 'Failed to load links');
    } finally {
      setLoading(false);
    }
  }, [projectId]);

  useEffect(() => {
    if (projectId) {
      refresh();
    }
  }, [projectId, refresh]);

  const add = useCallback(async (payload) => {
    const created = await createLink(projectId, payload);
    setLinks((prev) => [...prev, created]);
    return created;
  }, [projectId]);

  const remove = useCallback(async (id) => {
    await deleteLink(id);
    setLinks((prev) => prev.filter((l) => l.id !== id));
  }, []);

  return {
    links,
    loading,
    error,
    refresh,
    add,
    remove
  };
}
