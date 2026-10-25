import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';

bootstrapApplication(App, appConfig)
  .then(() => {
    const loader = document.getElementById('app-loader');
    if (!loader) return;

    loader.style.transition = 'opacity 0.25s ease-out';
    loader.style.opacity = '0';
    loader.addEventListener('transitionend', () => loader.remove(), { once: true });
  })
  .catch((err) => console.error(err));
