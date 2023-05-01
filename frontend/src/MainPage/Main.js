import React, { useState, useEffect } from 'react'
import styles from './Main.module.scss';
import Items from '../Items/Items';
import Filters from './Filters';
import Map, { NavigationControl, Marker } from 'react-map-gl';
import maplibregl from 'maplibre-gl';
import 'maplibre-gl/dist/maplibre-gl.css';
import Pagination from './Pagination';
function Main() {
    const [items, setItems] = useState([]);
    const [totalItems, setTotalItems] = useState(0);
    const [filters, setFilters] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [searchValue, setSearchValue] = useState('');
    const [error, setError] = useState(null);
    const pageSize = 10;
    const [pageNumber, setPageNumber] = useState(1);

    function showFilters() {
        console.log(filters)
        setFilters(!filters);
    }
    const onChangeSearchInput = (event) => {
        setSearchValue(event.target.value);
    }
    function paginate(pageNumber) {
        setPageNumber(pageNumber);
        setIsLoading(true);
        fetch(`${url}/vacancies?PageNumber=${pageNumber}&PageSize=${pageSize}`, {
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('token')}`,
            },
        })
            .then((response) => {
                console.log(response);
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then((data) => {
                if (!Array.isArray(data.items)) {
                    throw new Error('Response data does not have an items array');
                }
                setItems(data.items);
                setTotalItems(data.totalItems);
                setIsLoading(false);
            })
            .catch((error) => {
                setError(error.message);
                setIsLoading(false);
            });
    };

    const url = 'http://localhost:5020/api';
    useEffect(() => {
        async function getVacancies() {
            setIsLoading(true);
            try {
                const response = await fetch(`${url}/vacancies?PageNumber=${pageNumber}&PageSize=${pageSize}`, {
                    headers: {
                        'Content-Type': 'application/json',
                        Authorization: `Bearer ${localStorage.getItem('token')}`,
                    },
                });
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const data = await response.json();
                if (!Array.isArray(data.items)) {
                    throw new Error('Response data does not have an items array');
                }
                setItems(data.items);
                setTotalItems(data.totalItems);
                console.log(items);
                setIsLoading(false);
            } catch (error) {
                setError(error.message);
                setIsLoading(false);
            }
        }
        getVacancies();
    }, []);
    const lastItemsIndex = pageNumber * pageSize;
    const firstItemsIndex = lastItemsIndex - pageSize;
    const currentItems = items.slice(firstItemsIndex, lastItemsIndex);
    return (
        <div className="clear">
            <div className={styles.wrapper}>
                {filters && (
                    <div className={styles.filters}>
                        <Filters
                            pageNumber={pageNumber}
                            pageSize={pageSize}
                            showFilters={showFilters}
                        />
                    </div>
                )}

                <div className={styles.main}>
                    <div className={styles.content}>
                        <nav className={styles.search}>
                            <input onChange={onChangeSearchInput} value={searchValue} className={styles.input} placeholder="Search..." />
                            <img onClick={showFilters} className={styles.settings} src='./img/Rectangle 22.png' alt='filter' />
                        </nav>
                        <aside className={styles.aside}>
                            <div>
                                {currentItems.length > 0 && currentItems.filter(items => items.name.toLowerCase().includes(searchValue.toLowerCase()))
                                    .map((item, index) => (
                                        <Items
                                            key={index}
                                            id={item.gid}
                                            name={item.name}
                                            skills={item.skills}
                                            location={item.location}
                                            salary={item.salary}
                                        />
                                    ))}
                            </div>
                        </aside>
                        <div className={styles.pagination}>
                            <Pagination
                                pageSize={pageSize}
                                totalItems={items.length}
                                paginate={paginate}
                            />
                        </div>
                    </div>
                </div>
                <main className={styles.map}>
                    <Map mapLib={maplibregl}
                        initialViewState={{
                            longitude: 30.4222701,
                            latitude: 50.446638,
                            zoom: 14
                        }}
                        style={{ width: "100%", height: " 100%" }}
                        mapStyle="https://api.maptiler.com/maps/streets-v2/style.json?key=H2he3gVAuTjVermVNqo6	"
                    >
                        <NavigationControl position="top-right" />
                        {items.map((item, index) => {
                            const [lat, lng] = item.location.split('&');
                            return (
                                <Marker
                                    key={index}
                                    longitude={parseFloat(lng)}
                                    latitude={parseFloat(lat)}
                                    color="#61dbfb"
                                />
                            );
                        })}
                    </Map>
                </main>
            </div>
        </div >

    );

}

export default Main;
