import style from './Registration.module.scss'
import { json, Link } from 'react-router-dom'
import { useState } from "react"
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
function Registration() {
    const prevDef = (e) => {
        e.preventDefault();
        if (checkOut()) {
            const formData = {
                userName: userName,
                email: email,
                password: password,
                roleGid: null
            };
            console.log('Data:', formData);
            fetch('http://localhost:5010/api/users/sign-up', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(formData)
            })
                .then(response => {
                    return response;
                })
                .then(data => {
                    localStorage.setItem('token', data.token);
                    console.log(data);
                })
                .catch(error => console.error(error));
        }


    }
    function getUserName(event) {
        setUserName(event.target.value);
    }

    function getPassword(event) {
        setPassword(event.target.value);
    }

    function getEmail(event) {
        setEmail(event.target.value);
    }
    function getConfirmPassword(event) {
        setconfirmPassword(event.target.value);
    }

    const checkOut = () => {
        let check = true;
        if (userName === '' || userName === null) {
            check = false;
            toast.warning('Enter something in username');
        }
        if (password === '' || password === null) {
            check = false;
            toast.warning('Enter something in password');
        }
        if (confirmPassword !== password || confirmPassword === null) {
            check = false;
            toast.warning('Passwords do not match');
        }
        return check;
    }

    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');
    const [email, setEmail] = useState('');
    const [confirmPassword, setconfirmPassword] = useState('');
    return (
        <div className='clear'>
            <div className={style.wrapper}>
                <Link to='/'>
                    <button className={style.goBack}>Go back</button>
                </Link>
                <div className={style.registration}>
                    <form onSubmit={prevDef}>
                        <ul>
                            <li className="mt-20">
                                <input value={userName}
                                    className={style.input}
                                    onChange={getUserName}
                                    placeholder="Username" />
                            </li>
                            <li className="mt-20">
                                <input value={email}
                                    className={style.input}
                                    onChange={getEmail}
                                    placeholder="Email" />
                            </li>
                            <li className="mt-20">
                                <input value={password}
                                    className={style.input}
                                    placeholder="Password"
                                    onChange={getPassword}
                                    type='password' />
                            </li>
                            <li className="mt-20">
                                <input value={confirmPassword}
                                    onChange={getConfirmPassword}
                                    className={style.input}
                                    placeholder="Confirm password"
                                    type='password' />
                            </li>
                            <li className="mt-40">
                                <button onClick={checkOut} className={style.button}>Submit</button>
                            </li>
                        </ul>
                    </form>
                    <img className={style.img} src='/img/OrgCoral_Ofc-02_Concept-04.jpg' alt='picture' />
                </div>
            </div>
            <ToastContainer autoClose={4000} position='top-center' />
        </div>
    )
}

export default Registration;