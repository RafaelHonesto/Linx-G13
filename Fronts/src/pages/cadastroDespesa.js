import React, { Component } from 'react'
import '../css/perfil.css';
import '../css/despesas.css';
import BarraLateral from '../components/barra'
import axios from 'axios';
import { parseJwt } from '../services/auth';
import Swal from 'sweetalert2';
import enfeiteModal from '../img/enfeiteModal.png'
import alert from '../img/alertaExcluir.png'

class despesas extends Component {

    constructor(props) {
        super(props);
        this.state = {
            listaTiposDespesa: [],
            listaDespesas: [],
            idTipoDespesa: '',
            nome: '',
            descricao: '',
            valor: '',
            data: new Date(),
            vazio: false
        }
    }

    deuCerto (){
        Swal.fire({
            icon: 'success',
            title: 'Despesa excluída',
            showConfirmButton: false,
            timer: 1500
        })

        const modal = document.getElementById('modal')
        modal.classList.remove('mostrar')
        const modal2 = document.getElementById('modalSair')
        modal2.classList.remove('mostrar')
    }

    listarDespesas = () => {
        axios('http://localhost:5000/api/Despesa', {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

        .then(response => {
            if (response.status === 200) {
                this.setState({ listaDespesas: response.data})

                console.log(response.data)

                if (response.data.length === 0) {
                    this.setState({ vazio: true })
                }
            }
        })
            .catch(erro => console.log(erro))
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

    diminuirTabela () {
        const tabela = document.getElementById('tabela')
        const titulo = document.getElementById('titulo')
        tabela.classList.add('diminuir')
        titulo.classList.add('tituloDespesa2')
    }

    cadastrarDespesa = (event) => {

        event.preventDefault();

        axios.post('http://localhost:5000/api/Despesa', {
            idTipoDespesa: this.state.idTipoDespesa,
            nome: this.state.nome,
            descricao: this.state.descricao,
            dataDespesa: this.state.data,
            valor: this.state.valor
        }, {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(response => {
                if (response.status === 201) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Despesa cadastrada',
                        showConfirmButton: false,
                        timer: 1500
                    })
                }

                this.setState({
                    nome: '', descricao: '', idTipoDespesa: '', valor: ''
                })

                this.listarDespesas();
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
        this.listarDespesas();

        if(parseJwt().Role === '3'){
            this.diminuirTabela();
        }
    }

    funcaoMudaState = (campo) => {
        this.setState({ [campo.target.name]: campo.target.value })
    }

    render() {
        return (
            <section className="bodyDespesa">
                <section className="corpoDespesas">
                    <form onSubmit={this.cadastrarDespesa} className="inputsDespesas">
                        <h4>Cadastro de despesas</h4>
                        <select name='idTipoDespesa' value={this.state.idTipoDespesa} onChange={this.funcaoMudaState} >
                            <option>Selecione o tipo da despesa</option>
                            <option value='1'>Energia</option>
                            <option value='2'>Água</option>
                            <option value='3'>Salário de funcionário</option>
                        </select>
                        <input placeholder="Nome" type="text" name='nome' value={this.state.nome} onChange={this.funcaoMudaState} />
                        <input placeholder="Valor" type="text" name='valor' value={this.state.valor} onChange={this.funcaoMudaState} />
                        <input placeholder="Descrição (opcional)" type="text" name='descricao' value={this.state.descricao} onChange={this.funcaoMudaState} />

                        <button type='submit'>Adicionar +</button>
                    </form>

                    <div id='tabela' className="listaDespesas">
                        <table className="tabela-despesas">
                            <div id='titulo' className='tituloDespesas'>
                                <th>Nome</th>
                                <th>Data</th>
                                <th>Descrição</th>
                            </div>

                            <td className="vazioCDespesas" hidden={this.state.vazio === true ? false : true}>Não há nenhuma despesa cadastrada nesse setor</td>

                            {
                                this.state.listaDespesas.map((dados) => {
                                    return (
                                        <tr className='trDespesa'>
                                            <td> {dados.nome} </td>
                                            <td> {new Intl.DateTimeFormat('pt-BR').format(new Date(dados.dataDespesa))} </td>
                                            <td>{dados.descricao} </td>
                                            <button hidden={parseJwt().Role === '3' ? true : false} onClick={this.abreModal}>Editar</button>
                                        </tr>
                                    )
                                })
                            }
                            
                        </table>
                    </div>

                </section>

                <section id="modal" className="modal">
                    <div className="modal-containerDespesa">
                        <img src={enfeiteModal} alt='enfeite modal' className="imagemEnfeiteModal" />
                        <div className="content-modalDespesaPrincipal">
                            <div className="content-modalDespesa ">
                                <div className="inputsValor">
                                    <input placeholder="Nome" name='nomeGestor' value={this.state.nomeGestor} onChange={this.funcaoMudaState} type="text" />
                                    <input placeholder="Valor" name='acesso' value={this.state.acesso} onChange={this.funcaoMudaState} type="text" />
                                    <input placeholder="Descrição" name='senha' value={this.state.senha} onChange={this.funcaoMudaState} type="password" />
                                </div>
                                <form className="inputsValor">
                                    <input placeholder="Novo nome" name='nomeSetor' value={this.state.nomeSetor} onChange={this.funcaoMudaState} type="text" />
                                    <input placeholder="Novo valor" name='nomeGestor' value={this.state.nomeGestor} onChange={this.funcaoMudaState} type="text" />
                                    <input placeholder="Nova descrição" name='cpf' value={this.state.cpf} onChange={this.funcaoMudaState} type="text" />
                                </form>
                            </div>
                            
                            <div className="buttonsDespesa">
                                <button onClick={this.certezaExcluir}>Excluir despesa </button>
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
        );

    }
}

export default despesas;
