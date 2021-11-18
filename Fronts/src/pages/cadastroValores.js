import React, { Component } from 'react'
import '../css/perfil.css';
import '../css/valores.css';
import BarraLateral from '../components/barra'
import axios from 'axios';
import { parseJwt } from '../services/auth';
import Swal from 'sweetalert2';
import enfeiteModal from '../img/enfeiteModal.png'
import alert from '../img/alertaExcluir.png'

class valores extends Component {

    constructor(props) {
        super(props);
        this.state = {
            listaTiposDespesa: [],
            listaValor: [],
            tipoEntrada: true,
            titulo: '',
            idEmpresa: '',
            descricao: '',
            valor: '',
            data: new Date(),
            foto: 'default.png',
            listaEmpresa: [],
            vazio : false
        }
    }

    diminuirTabela() {
        const tabela = document.getElementById('tabela')
        const titulo = document.getElementById('titulo')
        const linha = document.getElementById('linha')
        tabela.classList.add('diminuir')
        titulo.classList.add('tituloValor2')
        linha.classList.add('trValor2')
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

    deuCerto() {
        Swal.fire({
            icon: 'success',
            title: 'Valor excluído',
            showConfirmButton: false,
            timer: 1500
        })

        const modal = document.getElementById('modal')
        modal.classList.remove('mostrar')
        const modal2 = document.getElementById('modalSair')
        modal2.classList.remove('mostrar')
    }

    selecionarEntrada = async (event) => {

        await this.setState({ tipoEntrada: true })

        event.preventDefault();

        const button = document.getElementById('button1')
        const button2 = document.getElementById('button2')
        button.classList.add('mudarCor')
        button2.classList.remove('mudarCor')
    }

    selecionarSaida = async (event) => {

        await this.setState({ tipoEntrada: false })

        event.preventDefault();

        const button = document.getElementById('button2')
        const button2 = document.getElementById('button1')
        button.classList.add('mudarCor')
        button2.classList.remove('mudarCor')
    }

    listarValores = () => {
        axios('http://localhost:5000/api/Valor', {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(response => {
                if(response){
                    this.setState({ listaValor: response.data })

                    if (response.data.length === 0) {
                        this.setState({ vazio: true })
                    }
                }
            } )

            .catch(erro => {
                if(erro){
                    console.log(erro)
                    this.setState({ vazio: true })
                }
            } )
    }

    cadastrarValores = (event) => {

        event.preventDefault();

        axios.post('http://localhost:5000/api/Valor', {
            tipoEntrada: this.state.tipoEntrada,
            valor: this.state.valor,
            dataValor: this.state.data,
            idEmpresa: this.state.idEmpresa,
            titulo: this.state.titulo,
            descricao: this.state.descricao,
            foto: this.state.foto
        }, {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(response => {
                if (response.status === 200) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Valor cadastrado',
                        showConfirmButton: false,
                        timer: 1500
                    })
                }

                this.setState({
                    tipoEntrada: "",
                    valor: "",
                    dataValor: "",
                    idEmpresa: "",
                    titulo: "",
                    descricao: "",
                    foto: ""
                })

                this.listarValores();
            })

            .catch(erro => console.log(erro))
    }

    ListarEmpresas() {
        axios('http://localhost:5000/api/Empresa', {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(response => this.setState({ listaEmpresa: response.data }))

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
        this.listarValores();

        if (parseJwt().Role === '3') {
            this.diminuirTabela();
        }

        this.ListarEmpresas();
    }

    funcaoMudaState = (campo) => {
        this.setState({ [campo.target.name]: campo.target.value })
    }

    render() {
        return (
            <section className="bodyDespesa">
                <section className="corpoValor">
                    <form onSubmit={this.cadastrarValores} className="inputsValores">
                        <h4>Cadastro de valores</h4>
                        <div className='buttonTipoValores'>
                            <button onClick={this.selecionarEntrada} id='button1' className="buttonTipoValores2">Entrada</button>
                            <button className="buttonTipoValores3" id='button2' onClick={this.selecionarSaida}>Saída</button>
                        </div>
                        <input placeholder="Titulo" type="text" name='titulo' value={this.state.titulo} onChange={this.funcaoMudaState} />
                        <input placeholder="Valor" type="text" name='valor' value={this.state.valor} onChange={this.funcaoMudaState} />
                        <select name='idEmpresa' value={this.state.idEmpresa} onChange={this.funcaoMudaState}>
                            <option value="0">Selecione uma empresa</option>
                            {
                                this.state.listaEmpresa.map((dados) => {
                                    return (
                                        <option value={dados.idEmpresa}>{dados.nomeEmpresa}</option>
                                    )
                                })
                            }
                        </select>
                        <input placeholder="Descrição (opcional)" type="text" name='descricao' value={this.state.descricao} onChange={this.funcaoMudaState} />
                        <div className="contentComprovanteValores">
                            <h4>Comprovante</h4>
                            <input placeholder="Comprovante" type="file" id='file' onChange={this.funcaoMudaState} />
                            <label for="file" className="inputImagemValores">Escolha uma imagem</label>
                        </div>

                        <button type='submit' className="buttonValor">Adicionar +</button>
                    </form>

                    <div id='tabela' className="listaValores">
                        <table className="tabela-despesas">
                            <div id='titulo' className='tituloValores'>
                                <th>Tipo</th>
                                <th>Data</th>
                                <th>Titulo</th>
                                <th>Empresa</th>
                                <th>Valor</th>
                            </div>

                            <td className="vazioValor" hidden={this.state.vazio === true ? false : true}>Não há nenhum valor cadastrado nesse setor</td>

                            {
                                this.state.listaValor.map((dados) => {
                                    return (
                                        <tr className='trValor'>
                                            <td> {dados.tipoEntrada === true ? 'Entrada' : 'Saída'} </td>
                                            <td> {new Intl.DateTimeFormat('pt-BR').format(new Date(dados.dataValor))} </td>
                                            <td>{dados.titulo} </td>
                                            <td>{dados.idEmpresaNavigation.nomeEmpresa} </td>
                                            <td>R${dados.valor} </td>
                                            <button hidden={parseJwt().Role === '3' ? true : false}>Editar</button>
                                        </tr>
                                    )
                                })
                            }

                            {/* <tr id='linha' className='trValor'>
                                <td> Entrada </td>
                                <td> 30/06/03 </td>
                                <td> Cadeira gamer </td>
                                <td> InfoTec </td>
                                <td> R$ 400 </td>
                                <button hidden={parseJwt().Role === '3' ? true : false} onClick={this.abreModal}>Editar</button>
                            </tr>
                            <tr id='linha' className='trValor'>
                                <td> Saida </td>
                                <td> 30/06/03 </td>
                                <td> Forno microondas </td>
                                <td> Casas Bahua </td>
                                <td> R$1200,00 </td>
                                <button hidden={parseJwt().Role === '3' ? true : false} onClick={this.abreModal}>Editar</button>
                            </tr> */}

                        </table>
                    </div>

                </section>

                <section id="modal" className="modal">
                    <div className="modal-containerAdm2">
                        <img src={enfeiteModal} alt='enfeite modal' className="imagemEnfeiteModal" />
                        <div className="content-modalValorPrincipal">
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
                                <button onClick={this.certezaExcluir}>Excluir valor </button>
                                <button>Atualizar</button>
                                <button id='fechar'>Cancelar</button>
                            </div>

                        </div>
                    </div>
                </section>

                <section id="modalSair" className="modal">
                    <div className="modal-containerSair">
                        <div className="content-modalSair">
                            <img src={alert} className='imagemSair' alt='simbolo de alerta' />
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
        );

    }
}

export default valores;
