import './css/login.css'
import React, { Component } from 'react'
import enfeiteLogin1 from './img/enfeiteLogin.png'
import VLibras from "@djpfs/react-vlibras";



class Login extends Component {

  render() {
    return (
      <div>
        <div className='App'>
          <VLibras/>
          <header className='App-header'>
            <img src={logo} className='App-logo' alt='logo'/>
          </header>
        </div>
        <section className="telaToda">
          <section className="container-login">
            <div className="content-login">
              <h1>Login</h1>
              <input placeholder="Acesso" type="text" />
              <input placeholder="Senha de acesso" type="text" />
              <button>Login</button>
              <p>Caso não tenha um acesso, entre em contato com o gestor da área financeira do seu setor</p>
            </div>
          </section>
          <img src={enfeiteLogin1} alt="enfeite login" className="imagemEnfeiteLogin" />
        </section>
      </div>
    );

  }
}

export default Login;
