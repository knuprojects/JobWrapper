import React from 'react'
import styles from '../MainPage/Main.module.scss';


function Items({ name, skills, location, salary, id }) {
    return (
        <div className={styles.item} key={id}>
            <h3 className={styles.item__name}>Позиція: {name}</h3>
            <p className={styles.item__desc}>
                <span className={styles.skills }>
                    <span className={styles.item__skills-title}>Навички:</span>{skills.join(', ')}
                </span>
                <span className={styles.filtersInput}>{salary && <>
                    <span className={styles.item__salary-title}></span>Заробітня плата: {salary}
                </>}
                </span>
            </p>
        </div>
    )
}

export default Items;
