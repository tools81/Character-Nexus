import 'bootstrap/dist/css/bootstrap.min.css';

import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import { Provider } from 'react-redux';
import { store } from './store/configureStore';
import App from './App';
import registerServiceWorker from './registerServiceWorker';

// Get the root DOM element
const rootElement = document.getElementById('root');

if (rootElement) {
  ReactDOM.render(
    <React.StrictMode>
      <Provider store={store}>
        <BrowserRouter>
          <App />
        </BrowserRouter>
      </Provider>
    </React.StrictMode>,
    rootElement
  );
} else {
  console.error("Could not find root element to mount React app");
}

// Optional: service worker
registerServiceWorker();
