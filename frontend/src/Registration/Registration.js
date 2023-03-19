import style from './Registration.module.scss'
import {  Link } from 'react-router-dom'
import { useState } from "react"
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
function Registration() {
    const prevDef = (e) =>{
        e.preventDefault();
        if(checkOut()){
            console.log('yep');
        }
    }
    
    const checkOut = () =>{
        if (userName === '' || userName === null ){
            toast.warning('Enter something in username');
        }
        if (password === '' || password === null){
            toast.warning('Enter something in password');
        }
        if ( confirmPassword ===  password || confirmPassword === null){
            toast.warning('Passwords do not match');
        }
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
                                <input value={userName} className={style.input} placeholder="Username" />
                            </li>
                            <li className="mt-20">
                                <input value={email} className={style.input} placeholder="Email" />
                            </li>
                            <li className="mt-20">
                                <input value = {password} className={style.input} placeholder="Password" type='password' />
                            </li>
                            <li className="mt-20">
                                <input value = {confirmPassword} className={style.input} placeholder="Confirm password" type='password' />
                            </li>
                            <li className="mt-40">
                                <button className={style.button}>Submit</button>
                            </li>
                        </ul>
                    </form>
                    <img className={style.img} src='/img/OrgCoral_Ofc-02_Concept-04.jpg' alt='picture' />
                </div>
            </div>
            <ToastContainer autoClose={4000} position='top-center'/>
        </div>
    )
}

export default Registration;