import React, { useState, useEffect } from 'react'
import Header from './Header';
import styles from './Main.module.scss';
import { vacancies } from './Data.js'
import Items from '../Items/Items';
function Main() {
    const [currentPage, setCurrentPage] = useState(1);
    const [pageNumber, setPageNumber] = useState(1);
    const [token, setToken] = useState('');
    const [vacancies,setVacancies] = useState([]);
    const pageSize = 10;

    useEffect(() => {
        setToken(localStorage.getItem('token'));
        getItems();
    }, []);
    async function getItems(pageNumber) {
        try {
            const response = await
                fetch(`http://localhost:5020/api/vacancies?PageNumber=${pageNumber}&PageSize=${pageSize}`,
                    {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json",
                            "Authorization": `Bearer ${token}`
                        }
                    });
            const items = await response.json();
            setVacancies(items);
        }
        catch (error) {
            console.log(error.message);
        }
        console.log(vacancies);
    }
    return (
        <div className={styles.main}>
            <header className={styles.header}>
                <Header />
            </header>
            <div className={styles.content}>
                <nav className={styles.sea}>
                    <input placeholder="Search..."></input>
                </nav>
                <aside className={styles.aside}>
                    <div className={styles.items}>
                        {vacancies.map((items) => (
                                <Items
                                    id = {items.gid}
                                    name={items.name}
                                    skills={items.skills}
                                    location={items.location}
                                    salary={items.salary}
                                />
                        ))}
                    </div>
                </aside >

            </div>
            <main className={styles.map}>
            </main>

        </div>
    )
}

export default Main;