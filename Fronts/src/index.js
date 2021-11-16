import React from 'react';
import ReactDOM from 'react-dom';
import Login from './pages/login';
import perfil from './pages/perfil';
import despesas from './pages/cadastroDespesa';
import reportWebVitals from './reportWebVitals';
import { Switch, BrowserRouter as Router, Route } from 'react-router-dom';
import telaAdm from './pages/telaADM';
import telaInicio from './pages/telaInicio';
import valores from './pages/cadastroValores';

const rotas = (
  <Router>
    <Switch>
      <Route exact path = "/" component = {Login}/>
      <Route path = "/perfil" component = {perfil}/>
      <Route path = "/despesas" component = {despesas}/>
      <Route path = "/adm" component = {telaAdm}/>
      <Route path = "/home" component = {telaInicio}/>
      <Route path = "/valores" component = {valores}/>
    </Switch>
  </Router>
)

ReactDOM.render(rotas, document.getElementById('root'));

reportWebVitals();
