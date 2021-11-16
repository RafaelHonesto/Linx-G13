import React, { Component } from "react";
import '../css/perfil.css';
import fotoPerfil from '../img/fotoPerfil.png'
import enfeiteModal from '../img/enfeiteModal.png'

import BarraLateral from '../components/barra'
import EnfeiteTela from '../components/enfeiteTela';
import axios from "axios";
import { parseJwt } from "../services/auth";
import Swal from "sweetalert2";

class perfil extends Component {

    constructor(props) {
        super(props);
        this.state = {
            listaUsuarios: [],
            acesso:'',
            senha:'',
            nome:'',
            cpf:'',
            funcao:'',
            foto: 'user.png',
            nomeFuncionario:'',
            cpfFuncionario: '',
            funcaoFuncionario:''
        }
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

    listarUsuarios = async () => {
        await axios('http://localhost:5000/api/Usuario/Setor', {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(response => this.setState({ listaUsuarios: response.data }))
    }

    cadastrarFuncionario = (event) => {

        event.preventDefault();

        axios.post('http://localhost:5000/api/Usuario/Gestor', {
            usuario: {
                acesso: this.state.acesso,
                senhaDeAcesso: this.state.senha
            },
            funcionario: {
                nome: this.state.nome,
                cpf: this.state.cpf,
                foto: this.state.foto,
                funcao: this.state.funcao
            }
        },{
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

        .then(response => {
            if(response.status === 201) {
                Swal.fire({
                    icon: 'success',
                    title: 'Novo funcionário cadastrado no sistema',
                    showConfirmButton: false,
                    timer: 1500
                })

                const modal = document.getElementById('modal')
                modal.classList.remove('mostrar')

                this.listarUsuarios();
            } 
        })

        .catch(erro => console.log(erro))
    }

    salvarImagem = (event) => {

        event.preventDefault();

        let formData = new FormData();
        formData.append("files", event.target.files[0]);
        axios.post('https://localhost:5001/api/FileUploads', formData, {
            headers: {
            'Content-Type': 'multipart/form-data'
            }
        })

        .then(resposta => {
            if(resposta.status === 200){
                Swal.fire({
                    imageUrl: 'http://localhost:5000/uploads/'+ this.state.foto ,
                    imageHeight: 200,
                    imageWidth: 200,
                    imageAlt: 'imagem de perfil'
                  })
            }
        })

        .catch(erro => console.log(erro));

    }

    funcaoMudaState = (campo) => {
        this.setState({ [campo.target.name]: campo.target.value })
    }

    buscarUsuario () {
        axios('http://localhost:5000/api/Usuario/Buscar',{
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

        .then(response => this.setState({nomeFuncionario: response.data.nome, cpfFuncionario:response.data.cpf, funcaoFuncionario:response.data.funcao}))
    }

    componentDidMount() {
        this.listarUsuarios();
        this.buscarUsuario();
    }

    render() {
        return (
            <section>
                <section className="corpoPerfil">
                    <div className="contentFotoPerfil">
                        <div className="FotoPerfil">
                            <img src={'http://localhost:5000/wwwroot/uploads/'+this.state.foto} alt="foto do perfil" className="fotoPerfil" />
                        </div>
                    </div>
                    <div className="contentNomePerfil">
                        <div className="NomePerfil">
                            <h1>{this.state.nomeFuncionario}</h1>
                            <h3>{this.state.cpfFuncionario}</h3>
                            <h3>{this.state.funcaoFuncionario}</h3>
                        </div>
                    </div>
                    <div className="contentModaisPerfil">
                        <div className="modalPerfil">
                            <button onClick={this.abreModal} hidden={parseJwt().Role === '3' ? true : false}><h3 >Cadastrar novo usuário ao sistema</h3></button>
                            <button onClick={this.abreModal2} hidden={parseJwt().Role === '3' ? true : false}><h3>Ver lista de usuários</h3></button>
                        </div>
                    </div>
                </section>

                <BarraLateral />

                <EnfeiteTela />

                <section id="modal2" className="modal">
                    <div className="modal-container">
                        <img src={enfeiteModal} alt='enfeite modal' className="imagemEnfeiteModal" />
                        <div className="content-modal2">
                            <table className="tabela-perfil">
                                <tr>
                                    <th>Nome </th>
                                    <th>Função</th>
                                    <th>CPF</th>
                                </tr>
                                    {
                                        this.state.listaUsuarios.map((dados) => {
                                            return (
                                                <tr>
                                                    <td> {dados.nome} </td>
                                                    <td> {dados.funcao} </td>
                                                    <td> {dados.cpf} </td>
                                                    <button>Editar</button>
                                                </tr>
                                            )
                                        })
                                    }

                            </table>

                        </div>
                    </div>
                </section>

                <section id="modal" className="modal">
                    <div className="modal-container2">
                        <img src={enfeiteModal} alt='enfeite modal' className="imagemEnfeiteModal" />
                        <form onSubmit={this.cadastrarFuncionario} className="content-modal">
                            <div className="inputsPerfil">
                                <input placeholder="Nome" type="text" name='nome' value={this.state.nome} onChange={this.funcaoMudaState}/>
                                <input placeholder="Acesso" type="text" name='acesso' value={this.state.acesso} onChange={this.funcaoMudaState}/>
                                <input placeholder="Senha de acesso" type="password" name='senha' value={this.state.senha} onChange={this.funcaoMudaState}/>
                                <input placeholder="CPF" type="text" name='cpf' value={this.state.cpf} onChange={this.funcaoMudaState}/>
                                <select placeholder="Função" name='funcao' value={this.state.funcao} type="text" onChange={this.funcaoMudaState}>
                                    <option>Selecione a função</option>
                                    <option value='Representante de finanças'>Representante de finanças</option>
                                </select>
                            </div>

                            <div className="area-perfil">
                                <div className="FotoPerfil2">
                                    <img src={fotoPerfil} alt="foto do perfil" className="fotoPerfil" />
                                </div>

                                <button className="button-foto">Fazer upload da foto</button>
                                <button className="button-foto2" type='submit'>Cadastrar</button>
                            </div>
                        </form>
                    </div>
                </section>
            </section>
        )
    }
}

export default perfil;