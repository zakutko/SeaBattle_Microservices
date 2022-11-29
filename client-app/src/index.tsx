import ReactDOM from 'react-dom/client';
import "semantic-ui-css/semantic.min.css";
import App from "./app/layout/App";
import { BrowserRouter as Router } from "react-router-dom";
import { store, StoreContext } from './app/stores/store';
import reportWebVitals from './reportWebVitals';
import { Container } from 'semantic-ui-react';
import "../src/app/layout/style.css";

const container = document.getElementById("root");
const root = ReactDOM.createRoot(container as HTMLElement);

root.render(
  <StoreContext.Provider value={store}>
    <div className="backgroud">
      <Container>
        <Router>
          <App />
        </Router>
      </Container>
    </div>
  </StoreContext.Provider>
);

reportWebVitals();