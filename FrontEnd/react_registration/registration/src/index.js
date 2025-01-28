import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { IsAuthenticatedProvider } from './Contexts/IsAuthenticatedContext';
import { CategoryProvider } from './Contexts/CategoryContext';


const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <IsAuthenticatedProvider>
      <CategoryProvider>
        <App />
      </CategoryProvider>
    </IsAuthenticatedProvider>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
