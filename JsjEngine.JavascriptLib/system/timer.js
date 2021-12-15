import { InsertTimerCallback, RemoveTimerCallback } from '@jsj/internal/timer';

export function setTimeout(func, delay) { return InsertTimerCallback(func, delay, false); }
export function setInterval(func, delay) { return InsertTimerCallback(func, delay, true); }
export function clearTimeout(timeoutId) { RemoveTimerCallback(timeoutId); }
export function clearInterval(timeoutId) { RemoveTimerCallback(timeoutId); }