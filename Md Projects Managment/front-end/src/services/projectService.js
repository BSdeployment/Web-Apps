import { apiFetch } from './api';

export function getProjects() {
  return apiFetch('/api/projects');
}

export function getProject(id) {
  return apiFetch(`/api/projects/${id}`);
}

export function createProject(payload) {
  return apiFetch('/api/projects', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });
}

export function updateProject(id, payload) {
  return apiFetch(`/api/projects/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });
}

export function deleteProject(id) {
  return apiFetch(`/api/projects/${id}`, { method: 'DELETE' });
}

export function getArticle(projectId) {
  return apiFetch(`/api/projects/${projectId}/article`);
}

export function upsertArticle(projectId, payload) {
  return apiFetch(`/api/projects/${projectId}/article`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });
}


export function sendArticleBeacon(projectId, markdownContent) {
  if (!navigator.sendBeacon) {
    return false;
  }
  const body = JSON.stringify({ markdownContent });
  const blob = new Blob([body], { type: 'application/json' });
  return navigator.sendBeacon(buildUrl(`/api/projects/${projectId}/article`), blob);
}
