import React, { Component } from 'react'
import enfeiteLogin from '../img/enfeiteLogin.png'
import enfeiteLogin2 from '../img/enfeiteLogin2.png'
import '../css/login.css'
import axios from 'axios';
import { parseJwt } from '../services/auth';
import Swal from 'sweetalert2'

class Login extends Component {
  constructor(props) {
    super(props);
    this.state = {
      email: '',
      senha: '',
      carregando: false
    }
  }

  login = (event) => {

    event.preventDefault();

    axios.post('http://localhost:5000/api/Usuario', {
      acesso: this.state.email,
      senhaDeAcesso: this.state.senha
    })

    .then(resposta => {
      if (resposta.status === 200) {
        localStorage.setItem('token-login', resposta.data.token)

        if (parseJwt().Role === '1') {

          this.props.history.push('/adm');

        } else {

          this.props.history.push('/home');

        }
      } 
    })

    .catch(erro => {
      if(erro){
        Swal.fire({
          icon: 'error',
          title: 'Eita...',
          text: 'Acesso ou senha incorretos, tente novamente'
      })
      this.setState({
        email: '',
        senha: ''
      })
    }
    })
  }

  funcaoMudaState = (campo) => {
    this.setState({ [campo.target.name]: campo.target.value })
  }


  render() {
    return (
      <section className="telaToda">
        <img src={enfeiteLogin2} alt="enfeite do login" className="imagemEnfeiteLogin2" />
        <section className="container-login">
          <form onSubmit={this.login} className="content-login">
            <h1>Login</h1>
            <input placeholder="Acesso" name="email" onChange={this.funcaoMudaState} value={this.state.email} type="text" />
            <input placeholder="Senha de acesso" name="senha" onChange={this.funcaoMudaState} value={this.state.senha} type="password" />
            <button type='submit'>Login</button>
            <p>Caso não tenha um acesso, entre em contato com o gestor da área financeira do seu setor</p>
          </form>
          <img src={enfeiteLogin} alt="enfeite login" className="imagemEnfeiteLogin" />
        </section>
      </section>
    );

  }
}

export default Login;
