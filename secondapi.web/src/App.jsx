import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import {Modal, ModalBody, ModalFooter, ModalHeader} from 'reactstrap';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import axios from 'axios';


function Home() {
    const baseUrl = "https://localhost:7146/API/Auth/login";

    const [data, setData] = useState([])

    const [usuario, setUsuario] = useState({
        email: '',
        senha: ''
    })

    const handleChange = e => {
        const { name, value } = e.target;
        setUsuario({
            ...usuario, [name]: value
        });
        console.log(usuario);
    }

    const requestAuth = async () => {
        const payload = {
            ...usuario
        };

        try {
            await axios.put(`${baseUrl}`, payload);

            setData(prev =>
                prev.map(usuario =>
                    usuario.senha === payload.senha ? { ...payload } : usuario
                )
            );
        } catch (error) {
            console.error("Erro Auth:", error);
        }
        };



    return (
        <div className="form-group">
            <h3>Bem vindo! Faça seu login</h3>
            <label>E-mail </label>
            <br />
            <input type="text" className="form-control" maxLength={50} style={{ width: '200px' }} name='email' onChange={ handleChange} />
            <br />
            <label>Senha </label>
            <br />
            <input type="password" className="form-control" style={{ width: '200px' }} name='senha' onChange={handleChange} />
            <br />
            <button className="btn btn-secondary" onClick={() => requestAuth()} >Login</button>{"   "}
        </div>
        
    );
}

function Contact() {
    return <h1>Contact Page</h1>;
}

function ListaLivros() {
    const baseUrl = "https://localhost:7146/API/Livros";

    const [data, setData] = useState([])

    const [modalIncluir, setModalIncluir] = useState(false);

    const [modalEditar, setModalEditar] = useState(false);

    const [modalExcluir, setModalExcluir] = useState(false);

    const [livroSelecionado, setLivroSelecionado] = useState({
        titulo: '',
        autor: '',
        ano: '',
        genero: ''
    })

    const handleChange = e => {
        const { name, value } = e.target;
        setLivroSelecionado({
            ...livroSelecionado, [name]: value
        });
        console.log(livroSelecionado);
    }

    const selecionarLivro = (livro, caso) => {
        setLivroSelecionado(livro);
        (caso === "Editar") ?
            abrirFecharModalEditar() : abrirFecharModalExcluir();
    }

    const abrirFecharModalEditar = () => {
        setModalEditar(!modalEditar);
    }

    const abrirFecharModalIncluir = () => {
        setModalIncluir(!modalIncluir);
    }

    const abrirFecharModalExcluir = () => {
        setModalExcluir(!modalExcluir);
    }


    const requestGet = async () => {
        await axios.get(baseUrl)
            .then(response => {
                setData(response.data);
            }).catch(error => {
                console.log(error);
            })
    }

    const requestPost = async () => {
        livroSelecionado.ano = parseInt(livroSelecionado.ano);
        await axios.post(baseUrl, [livroSelecionado])
            .then(response => {
                setData(data.concat(response.data));
                requestGet();
                abrirFecharModalIncluir();
            }).catch(error => {
                console.log(error);
            })
    }


    const requestPut = async () => {
        const payload = {
            ...livroSelecionado,
            ano: parseInt(livroSelecionado.ano)
        };

        try {
            await axios.put(`${baseUrl}/${livroSelecionado.id}`, payload);

            setData(prev =>
                prev.map(livro =>
                    livro.id === payload.id ? { ...payload } : livro
                )
            );

            abrirFecharModalEditar();
        } catch (error) {
            console.error("Erro no PUT:", error);
        }
    };

    const pedidoDelete = async () => {
        await axios.patch(baseUrl + "/" + livroSelecionado.id)
            .then(() => {
                requestGet();
                abrirFecharModalExcluir();
            }).catch(error => {
                console.log(error);
            })
    }

    useEffect(() => {
        requestGet();
    }, []);

    return (

        <div className="App">
            <br></br>
            <h3>Cadastro de livros</h3>
            <header>
                <button onClick={() => abrirFecharModalIncluir()} className="btn btn-success">Incluir novo livro</button>
            </header>
            <br></br>
            <table className="table table-bordered" >

                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Titulo</th>
                        <th>Autor</th>
                        <th>Ano</th>
                        <th>Genero</th>
                        <th>Acoes</th>
                    </tr>
                </thead>
                <tbody>

                    {data.map(livro => (
                        <tr key={livro.id}>
                            <td>{livro.id}</td>
                            <td>{livro.titulo}</td>
                            <td>{livro.autor}</td>
                            <td>{livro.ano}</td>
                            <td>{livro.genero}</td>
                            <td>
                                <button className="btn btn-primary" onClick={() => selecionarLivro(livro, "Editar")}>Editar</button> {" "}
                                <button className="btn btn-danger" onClick={() => selecionarLivro(livro, "Excluir")}>Excluir</button> {" "}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>


            <Modal isOpen={modalIncluir}>
                <ModalHeader>Incluir livros</ModalHeader>
                <ModalBody>
                    <div className="form-group">
                        <label>Titulo: </label>
                        <br />
                        <input type="text" className="form-control" name='titulo' onChange={handleChange} />
                        <br />
                        <label>Autor: </label>
                        <br />
                        <input type="text" className="form-control" name='autor' onChange={handleChange} />
                        <br />
                        <label>Ano: </label>
                        <br />
                        <input type="text" className="form-control" name='ano' onChange={handleChange} />
                        <br />
                        <label>Genero: </label>
                        <br />
                        <input type="text" className="form-control" name='genero' onChange={handleChange} />
                        <br />
                    </div>
                </ModalBody>
                <ModalFooter>
                    <button className="btn btn-primary" onClick={() => requestPost()}>Incluir</button>{"   "}
                    <button className="btn btn-danger" onClick={() => abrirFecharModalIncluir()}>Cancelar</button>
                </ModalFooter>
            </Modal>

            <Modal isOpen={modalEditar}>
                <ModalHeader>Editar Livro</ModalHeader>
                <ModalBody>
                    <div className="form-group">
                        <label>ID: </label>
                        <br />
                        <input type="text" className="form-control" value={livroSelecionado.id} readOnly />
                        <br />
                        <label>Titulo: </label>
                        <br />
                        <input type="text" className="form-control" name="titulo" onChange={handleChange} value={livroSelecionado.titulo} />
                        <br />
                        <label>Autor: </label>
                        <br />
                        <input type="text" className="form-control" name="autor" onChange={handleChange} value={livroSelecionado.autor} />
                        <br />
                        <label>Ano: </label>
                        <br />
                        <input type="text" className="form-control" name="ano" onChange={handleChange} value={livroSelecionado.ano} />
                        <br />
                        <label>Genero: </label>
                        <br />
                        <input type="text" className="form-control" name="genero" onChange={handleChange} value={livroSelecionado.genero} />
                        <br />
                    </div>
                </ModalBody>
                <ModalFooter>
                    <button className="btn btn-primary" onClick={() => requestPut()}>Editar</button>{"   "}
                    <button className="btn btn-danger" onClick={() => abrirFecharModalEditar()}>Cancelar</button>
                </ModalFooter>
            </Modal>

            <Modal isOpen={modalExcluir}>
                <ModalBody>
                    Deseja excluir este livro: {livroSelecionado && livroSelecionado.titulo}?
                </ModalBody>
                <ModalFooter>
                    <button className="btn btn-danger" onClick={() => pedidoDelete()}> Sim</button>
                    <button className="btn btn-secondary" onClick={() => abrirFecharModalExcluir()}>Cancelar</button>
                </ModalFooter>
            </Modal>

        </div>


    );
}


function App() {
    return (
        <BrowserRouter>
            {/* Navigation */}
            <nav>
                <Link to="/">Login</Link> |{" "}
                <Link to="/Livros">Livros</Link> |{" "}
                <Link to="/contact">Login</Link>
            </nav>

            {/* Routes */}
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/Livros" element={<ListaLivros />} />
                <Route path="/contact" element={<Contact />} />
            </Routes>
        </BrowserRouter>
    );
}

export default App;