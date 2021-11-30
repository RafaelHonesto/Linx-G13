import React, { Component } from "react";
import '../css/telaAdm.css';
import enfeiteModal from '../img/enfeiteModal.png'
import BarraLateral from '../components/barra'
import axios from "axios";
import Swal from "sweetalert2";
import alert from '../img/alertaExcluir.png'

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
            idSetor: ""

        }

    }

    deuCerto (){
        Swal.fire({
            icon: 'success',
            title: 'Setor excluído',
            showConfirmButton: false,
            timer: 1500
        })

        const modal = document.getElementById('modal')
        modal.classList.remove('mostrar')
        const modal2 = document.getElementById('modalSair')
        modal2.classList.remove('mostrar')
    }

    abreModal = () => {
        const modal = document.getElementById('modal')
        modal.classList.add('mostrar')
        modal.addEventListener('click', (e) => {
            if (e.target.id === "modal" || e.target.id === "fechar") {
                modal.classList.remove('mostrar')
            }
        })
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

        event.preventDefault();

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

                    const modal = document.getElementById('modal2')
                    modal.classList.remove('mostrar')

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

                    this.buscarSetor();
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
                    this.setState({ setores: dados.data.length, listaSetores: dados.data})

                    console.log(dados)
                }
            })

            .catch(erro => console.log(erro))
    }

    certezaExcluir() {
        const modal = document.getElementById('modalSair')
        modal.classList.add('mostrar')
        modal.addEventListener('click', (e) => {
            if (e.target.id === "modalSair" || e.target.id === "fechar") {
                modal.classList.remove('mostrar')
            }
        })
    }

    componentDidMount() {
        this.buscarSetores()
    }

    funcaoMudaState = (campo) => {
        this.setState({ [campo.target.name]: campo.target.value })
    }

    somarTudo = (dados) => {
        let soma = 0

        dados.map((dado) => {
            if(dado.tipoEntrada){
                soma+= parseInt(dado.valor)
            } 
        })

        return soma
    }
    
    subtrairTudo = (dados) => {
        let saida = 0

        dados.map((dado) => {
            if(dado.tipoEntrada == false){
                saida+= parseInt(dado.valor)
            } 
        })

        return saida
    }


    render() {
        return (
            <section className='body'>
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

                        <thead className="titulo-tabela-adm">
                            <th>Nº de funcionários</th>
                            <th>Gestão</th>
                            <th>Entradas</th>
                            <th>Saídas</th>
                        </thead>
                        <tbody>
                        {
                            this.state.listaSetores.map((dados) => {
                                return (
                                    <tr className='linhaTabelaAdm'>
                                        <td>{dados.nome}</td>
                                        <td>{dados.funcionarios.length}</td>
                                        <td>Gabriel</td>
                                        <td>R$ {this.somarTudo(dados.valores)}</td>
                                        <td>R$ {this.subtrairTudo(dados.valores)}</td>
                                        <button onClick={this.abreModal}>Editar</button>
                                    </tr>
                                )
                            })
                        }
                        </tbody>
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
                                <input placeholder="Senha de acesso" name='senha' value={this.state.senha} onChange={this.funcaoMudaState} type="password" />
                                <button type="submit">Criar</button>
                            </form>
                        </div>
                    </div>
                </section>

                <section id="modal" className="modal">
                    <div className="modal-containerAdm2">
                        <img src={enfeiteModal} alt='enfeite modal' className="imagemEnfeiteModal" />
                        <div className="content-modalAdmPrincipal">
                            <div className="content-modalAdm">
                                <div className="inputsValor">
                                    <input placeholder="Nome do setor" name='nomeSetor' value={this.state.nomeSetor} onChange={this.funcaoMudaState} type="text" />
                                    <input placeholder="Nome do gestor" name='nomeGestor' value={this.state.nomeGestor} onChange={this.funcaoMudaState} type="text" />
                                    <input placeholder="CPF" name='cpf' value={this.state.cpf} onChange={this.funcaoMudaState} type="text" />
                                    <input placeholder="Acesso" name='acesso' value={this.state.acesso} onChange={this.funcaoMudaState} type="text" />
                                    <input placeholder="Senha de acesso" name='senha' value={this.state.senha} onChange={this.funcaoMudaState} type="password" />
                                </div>
                                <form className="inputsValor">
                                    <input placeholder="Novo nome do setor" name='nomeSetor' value={this.state.nomeSetor} onChange={this.funcaoMudaState} type="text" />
                                    <input placeholder="Novo nome do gestor" name='nomeGestor' value={this.state.nomeGestor} onChange={this.funcaoMudaState} type="text" />
                                    <input placeholder="Novo CPF" name='cpf' value={this.state.cpf} onChange={this.funcaoMudaState} type="text" />
                                    <input placeholder="Novo acesso" name='acesso' value={this.state.acesso} onChange={this.funcaoMudaState} type="text" />
                                    <input placeholder="Nova senha de acesso" name='senha' value={this.state.senha} onChange={this.funcaoMudaState} type="password" />
                                </form>
                            </div>
                            
                            <div className="buttonsDespesa">
                                <button onClick={this.certezaExcluir}>Excluir setor </button>
                                <button>Atualizar</button>
                                <button id='fechar'>Cancelar</button>
                            </div>

                        </div>
                    </div>
                </section>

                <section id="modalSair" className="modal">
                    <div className="modal-containerSair">
                        <div className="content-modalSair">
                            <img src={alert} className='imagemSair' alt='simbolo de alerta'/>
                            <h4>Você realmente deseja excluir?</h4>
                            <h3>Essa ação não poderá ser desfeita.</h3>
                            <div className='buttonsSair'>
                                <button className='botaoSair' onClick={this.deuCerto}>Sim, excluir</button>
                                <button id='fechar' className='botaoCancela'>Cancelar</button>
                            </div>
                        </div>
                    </div>
                </section>

                <BarraLateral />

            </section>
        )
    }
}

export default telaAdm;