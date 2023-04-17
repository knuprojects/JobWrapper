import React, { useState, useEffect } from 'react'
import Header from './Header';
import styles from './Main.module.scss';
import { data } from './Data.js'
import Items from '../Items/Items';
import Filters from './Filters';
function Main() {
    const [currentPage, setCurrentPage] = useState(1);
    const [pageNumber, setPageNumber] = useState(1);
    const [token, setToken] = useState('');
    const [vacancies, setVacancies] = useState([]);
    const [filters, setFilters] = useState(false);
    const pageSize = 10;

    useEffect(() => {
        setToken(localStorage.getItem('token'));
        // getItems();
    }, []);
    function showFilters() {
        setFilters(!filters);
    }
    // async function getItems(pageNumber) {
    //     try {
    //         const response = await
    //             fetch(`http://localhost:5020/api/vacancies?PageNumber=${pageNumber}&PageSize=${pageSize}`,
    //                 {
    //                     method: "POST",
    //                     headers: {
    //                         "Content-Type": "application/json",
    //                         "Authorization": `Bearer ${token}`
    //                     }
    //                 });
    //         const items = await response.json();
    //         setVacancies(items);
    //     }
    //     catch (error) {
    //         console.log(error.message);
    //     }
    //     console.log(vacancies);
    // }
    return (
        <div className="clear">
            {filters ?
                <div className={styles.filters}>
                    <Filters
                    showFilters = {showFilters}
                    />
                </div> : null}
            <div className={styles.main}>
                <div className={styles.content}>

                    <nav className={styles.search}>
                        <input className={styles.input} placeholder="Search..." />
                        <img onClick={showFilters} className={styles.photo} src='./img/Rectangle 22.png' width={38} height={40} alt='filter' />
                    </nav>
                    <aside className={styles.aside}>
                        <div>
                            {data.map((items) => (
                                <Items
                                    id={items.gid}
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
        </div>


    )
}

export default Main;