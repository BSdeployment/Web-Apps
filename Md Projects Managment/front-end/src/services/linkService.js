import { apiFetch } from './api';

export function getLinks(projectId) {
  return apiFetch(`/api/projects/${projectId}/links`);
}

export function createLink(projectId, payload) {
  return apiFetch(`/api/projects/${projectId}/links`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });
}

export function deleteLink(linkId) {
  return apiFetch(`/api/links/${linkId}`, { method: 'DELETE' });
}
