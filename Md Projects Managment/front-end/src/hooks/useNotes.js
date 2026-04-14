import { useCallback, useEffect, useState } from 'react';
import { getNote, upsertNote } from '../services/noteService';

export function useNotes(projectId) {
  const [note, setNote] = useState(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState(null);

  const refresh = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getNote(projectId);
      setNote(data);
    } catch {
      setNote(null);
    } finally {
      setLoading(false);
    }
  }, [projectId]);

  useEffect(() => {
    if (projectId) {
      refresh();
    }
  }, [projectId, refresh]);

  const save = useCallback(async (content) => {
    setSaving(true);
    try {
      const result = await upsertNote(projectId, { content });
      if (result) {
        setNote(result);
      } else {
        setNote(null);
      }
      return result;
    } finally {
      setSaving(false);
    }
  }, [projectId]);

  return {
    note,
    loading,
    saving,
    error,
    refresh,
    save
  };
}
