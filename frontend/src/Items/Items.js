import React from 'react'
import styles from '../MainPage/Main.module.scss';
function Items({ name, skills, location, salary, id }) {
    return (
        <div className={styles.item} key={id}>
            <ul>
                <li><h4>Позиція: {name}</h4></li>
                <li>  <h3>Навички: {skills.join(', ')}</h3> </li>
                <li> <h3> {salary && <>
                    Заробітня плата: {salary}
                </>}</h3></li>
            </ul>



        </div>
    )
}

export default Items;
{/* <h3>{location}</h3> */ }