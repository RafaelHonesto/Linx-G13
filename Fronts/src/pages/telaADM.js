import React, { Component } from "react";
import '../css/telaAdm.css';
import enfeiteModal from '../img/enfeiteModal.png'
import EnfeiteTelas from '../components/enfeiteTela';
import BarraLateral from '../components/barra'
import axios from "axios";
import Swal from "sweetalert2";

var data = new Date();
var dia = String(data.getDate()).padStart(2, '0');
var mes = String(data.getMonth() + 1).padStart(2, '0');
var ano = data.getFullYear();

var dataAtual = dia + '/' + mes + '/' + ano;

class telaAdm extends Component {

    constructor(props) {
        super(props);
        this.state = {
            acesso: '',
            senha: '',
            nomeSetor: '',
            nomeGestor: '',
            cpf: '',
            setores: [],
            listaSetores: [],
            foto: 'user.png',
            idSetor: ""
        }

    }

    abreModal2 = () => {
        const modal = document.getElementById('modal2')
        modal.classList.add('mostrar')
        modal.addEventListener('click', (e) => {
            if (e.target.id === "modal2" || e.target.id === "fechar") {
                modal.classList.remove('mostrar')
            }
        })
    }

    cadastrarSetor = async (event) => {

        event.preventDefault();

        await axios.post('http://localhost:5000/api/Setores', {
            nome: this.state.nomeSetor
        }, {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .catch(erro => (console.log(erro)))

        this.buscarSetor();

    }

    buscarSetor = async () => {

        await axios(`http://localhost:5000/api/Setores/buscar/${this.state.nomeSetor}`, {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(resposta => {
                if (resposta.status === 200) {
                    this.setState({
                        idSetor: resposta.data.idSetor
                    })
                }

                if (this.state.idSetor !== "") {
                    this.cadastrarUsuario();
                }
            })

            .catch(erro => console.log(erro))

        console.log(this.state.nomeSetor)
    }

    cadastrarUsuario = async (event) => {


        await axios.post('http://localhost:5000/api/Usuario/Criar', {
            usuario: {
                acesso: this.state.acesso,
                senhaDeAcesso: this.state.senha
            },
            funcionario: {
                idSetor: this.state.idSetor,
                nome: this.state.nomeGestor,
                cpf: this.state.cpf,
                foto: this.state.foto,
                funcao: "Gestor de finanças"
            }
        }, {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(resposta => {
                if (resposta.status === 201) {
                    this.setState({
                        nome: "",
                        cpf: "",
                        foto: "",
                        nomeGestor: "",
                        acesso: "",
                        senha: ""

                    })

                    Swal.fire({
                        icon: 'success',
                        title: 'Novo setor e gestor criados!',
                        showConfirmButton: false,
                        timer: 1500
                    })
                }
            })

            .catch(erro => (console.log(erro)))
    }

    buscarSetores() {
        axios('http://localhost:5000/api/Setores', {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

        .then(dados => {
            if (dados.status === 200) {
                this.setState({ setores: dados.data.length, listaSetores : dados.data })
            }
        })

        .catch(erro => console.log(erro))
    }

    componentDidMount() {
        this.buscarSetores()
    }

    funcaoMudaState = (campo) => {
        this.setState({ [campo.target.name]: campo.target.value })
    }


    render() {
        return (
            <section>
                <section className="corpoAdm">
                    <div className="totalSetores">
                        <div className="totalSetores-text">
                            <h4>
                                Total de setores
                            </h4>

                            <p>{dataAtual}</p>

                        </div>
                        <h5>
                            {this.state.setores}
                        </h5>
                    </div>

                    <table className="tabela-adm">
                        <h1>
                            Setores
                        </h1>

                        <div className="titulo-tabela-adm">
                            <th>Nº de funcionários</th>
                            <th>Total de entradas</th>
                            <th>Total de saída</th>
                            <th>Gestão</th>
                        </div>
                        {
                            this.state.listaSetores.map((dados) => {
                                return (
                                    <tr>
                                        <td>{dados.nome}</td>
                                        <td>45</td>
                                        <td>1</td>
                                        <td>2</td>
                                        <td>Gabriel</td>
                                        <button onClick={this.abreModal}>Editar</button>
                                    </tr>
                                )
                            })
                        }

                    </table>

                    <p onClick={this.abreModal2} className="cadastrarAdm">Cadastrar novos setores +</p>
                </section>

                <section id="modal2" className="modal">
                    <div className="modal-containerAdm">
                        <img src={enfeiteModal} alt='enfeite modal' className="imagemEnfeiteModal" />
                        <div className="content-modal">
                            <form className="inputsPerfil" onSubmit={this.cadastrarSetor}>
                                <input placeholder="Nome do setor" name='nomeSetor' value={this.state.nomeSetor} onChange={this.funcaoMudaState} type="text" />
                                <input placeholder="Nome do gestor" name='nomeGestor' value={this.state.nomeGestor} onChange={this.funcaoMudaState} type="text" />
                                <input placeholder="CPF" name='cpf' value={this.state.cpf} onChange={this.funcaoMudaState} type="text" />
                                <input placeholder="Acesso" name='acesso' value={this.state.acesso} onChange={this.funcaoMudaState} type="text" />
                                <input placeholder="Senha de acesso" name='senha' value={this.state.senhar} onChange={this.funcaoMudaState} type="text" />
                                <button type="submit">Criar</button>
                            </form>
                        </div>
                    </div>
                </section>

                <EnfeiteTelas />

                <BarraLateral />

            </section>
        )
    }
}

export default telaAdm;