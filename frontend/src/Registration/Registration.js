import style from './Registration.module.scss'
import { Route, Routes, Link } from 'react-router-dom'

function Registration() {
    return (
        <div className='clear'>
            <div className={style.wrapper}>
                <Link to='/'>
                    <button className={style.goBack}>Go back</button>
                </Link>
                <div className={style.registration}>
                    <ul>
                        <li className="mt-20">
                            <input className={style.input} placeholder="Username" /> 
                        </li>
                        <li className="mt-20">
                            <input className={style.input} placeholder="Email" />
                        </li>
                        <li className="mt-20">
                            <input className={style.input} placeholder="Password" type='password' />
                        </li>
                        <li className="mt-20">
                            <input className={style.input} placeholder="Confirm password" type='password'/>
                        </li>
                        <li className="mt-40">
                            <button className={style.button}>Submit</button>
                        </li>
                    </ul>
                    <img className={style.img} src='/img/OrgCoral_Ofc-02_Concept-04.jpg' alt='picture' />
                </div>
            </div>
        </div>
    )
}

export default Registration;