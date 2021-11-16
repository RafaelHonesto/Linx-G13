import { Component } from "react";
import { Link } from "react-router-dom";
import '../css/barraLateral.css'
import { parseJwt } from "../services/auth";
import alert from '../img/alerta.gif'

class BarraLateral extends Component {

    constructor(props) {
        super(props);
        this.state = {
            logout: false
        }
    }

    certezaSair() {
        const modal = document.getElementById('modalBarra')
        modal.classList.add('mostrar')
        modal.addEventListener('click', (e) => {
            if (e.target.id === "modalBarra" || e.target.id === "fechar") {
                modal.classList.remove('mostrar')
            }
        })
    }

    logout() {
        localStorage.removeItem('token-login');
    }



    render() {
        return (
            <section>

                <div class="container">
                    <ul>
                        <Link to='/home'><li className="li" >Início</li></Link>
                        <Link to='/valores'><li className="li" hidden={parseJwt().Role === '1' ? true : false}>Valores</li></Link>
                        <Link to='/despesas'><li className="li" hidden={parseJwt().Role === '1' ? true : false}>Despesas</li></Link>
                        <Link to='/home'><li className="li" hidden={parseJwt().Role === '1' ? true : false}>Dashboard</li></Link>
                        <Link to='/relatorio'><li className="li" hidden={parseJwt().Role !== '1' ? true : false}>Relatórios</li></Link>
                        <Link to='/perfil'><li className="li" hidden={parseJwt().Role === '1' ? true : false}>Perfil</li></Link>
                        <li onClick={this.certezaSair} className="li">Sair</li>
                    </ul>
                </div>

                <section id="modalBarra" className="modal">
                    <div className="modal-containerSair">
                        <div className="content-modalSair">
                            <img src={alert} alt='simbolo de alerta'/>
                            <h4>Você realmente deseja sair?</h4>
                            <div className='buttonsSair'>
                                <Link to='/'><button onClick={this.logout} className='botaoSair'>Sim, sair</button></Link>
                                <button id='fechar' className='botaoCancela'>Não</button>
                            </div>
                        </div>
                    </div>
                </section>
            </section>


        )
    }
}

export default BarraLateral