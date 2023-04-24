import React, { useState } from 'react'
import styles from './Main.module.scss';



const Filters = ({ showFilters }) => {
    const [minSalary, setMinSalary] = useState();
    const [maxSalary, setMaxSalary] = useState();
    const [skills, setSkills] = useState([]);
    function salaryInput(event) {
        const pattern = /[0-9]/;
        const input = String.fromCharCode(event.charCode);
        if (!pattern.test(input)) {
            event.preventDefault();
        }
    }

    function skillsInput(event) {
        setSkills(prevSkills => [...prevSkills, document.getElementById('skillInput').value]);
        console.log(skills);
    }



    return (
        <div className={styles.modal}>
            <img onClick={showFilters} className={styles.close} width={35} src='./img/close.png' />
            <ul>
                <li>
                    <div>
                        <p>Enter salary:</p>
                    </div>
                    <input onKeyPress={salaryInput} onChange={event => { setMinSalary(event.target.value) }} className={styles.filtersInput} placeholder=' Minimum salary' />
                    <input onKeyPress={salaryInput} onChange={event => { setMaxSalary(event.target.value) }} className={styles.filtersInput} placeholder=' Maximum salary' />
                </li>
                {minSalary && maxSalary && (
                    <p>You put: {minSalary} to {maxSalary} </p>
                )}
                <li className='d-flex align-center'>
                    <p>Enter skills</p>
                    <input id="skillInput" className={styles.filtersInput} placeholder=' Your skill:' />
                    <button className={styles.skills} onClick={skillsInput}>+</button>
                </li>
                {skills.length>0 && (
                    <p>You put: {skills.join(',')} </p>
                )}
                <button onClick={showFilters} className={styles.ok}>Оk</button>

            </ul>
        </div>
    )
}

export default Filters;
