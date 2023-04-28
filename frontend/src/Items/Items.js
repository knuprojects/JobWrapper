import React from 'react'
import styles from '../MainPage/Main.module.scss';


function Items({ name, skills, location, salary, id }) {
    return (
        <div className={styles.item} key={id}>
            <h3 className={styles.itemName}>Позиція: {name}</h3>
            <p className={styles.itemDesc}>
                <span className={styles.itemSkills }>
                    <span className={styles.itemskillsTitle}>Навички:</span>{skills.join(', ')}
                </span>
                <span className={styles.itemSalary}>{salary && <>
                    <span className={styles.itemSalaryTitle}></span>Заробітня плата: {salary}
                </>}
                </span>
            </p>
        </div>
    )
}

export default Items;
