import { apiFetch } from './api';

export function getNote(projectId) {
  return apiFetch(`/api/projects/${projectId}/notes`);
}

export function upsertNote(projectId, payload) {
  return apiFetch(`/api/projects/${projectId}/notes`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });
}

export function deleteNote(projectId) {
  return apiFetch(`/api/projects/${projectId}/notes`, { method: 'DELETE' });
}


export function sendNoteBeacon(projectId, content) {
  if (!navigator.sendBeacon) {
    return false;
  }
  const body = JSON.stringify({ content });
  const blob = new Blob([body], { type: 'application/json' });
  return navigator.sendBeacon(buildUrl(`/api/projects/${projectId}/notes`), blob);
}
