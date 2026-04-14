import { useCallback, useEffect, useState } from 'react';
import { getArticle, getProject, upsertArticle, updateProject } from '../services/projectService';

export function useProject(projectId) {
  const [project, setProject] = useState(null);
  const [article, setArticle] = useState(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState(null);

  const load = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const projectData = await getProject(projectId);
      setProject(projectData);
      try {
        const articleData = await getArticle(projectId);
        setArticle(articleData);
      } catch {
        setArticle(null);
      }
    } catch (err) {
      setError(err.message || 'Failed to load project');
    } finally {
      setLoading(false);
    }
  }, [projectId]);

  useEffect(() => {
    if (projectId) {
      load();
    }
  }, [projectId, load]);

  const saveArticle = useCallback(async (markdownContent) => {
    setSaving(true);
    try {
      const saved = await upsertArticle(projectId, { markdownContent });
      setArticle(saved);
      return saved;
    } finally {
      setSaving(false);
    }
  }, [projectId]);

  const toggleCompleted = useCallback(async () => {
    if (!project) {
      return null;
    }
    const updated = await updateProject(projectId, {
      title: project.title,
      description: project.description,
      completed: !project.completed
    });
    setProject(updated);
    return updated;
  }, [project, projectId]);

  return {
    project,
    article,
    loading,
    saving,
    error,
    refresh: load,
    saveArticle,
    toggleCompleted
  };
}
