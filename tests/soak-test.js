import http from 'k6/http';
import { sleep } from 'k6';
import * as config from './config.js';

export const options = {
    stages: [
        { duration: '30s', target: 16},
        { duration: '1h', target: 16},
        { duration: '10m', target: 5},
        { duration: '5s', target: 0}
    ],

    thresholds: {
        http_req_duration: ['p(95)<1000']
    }
}

export default function () {
  http.get(config.API_VACANCIES,
    {
        headers: {
            accept: 'application/json',
            authorization: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyODFkY2E0ZC01MWQ3LTRmY2YtYTU3NS1lYmY0OTJmY2Q4NjQiLCJ1bmlxdWVfbmFtZSI6IjI4MWRjYTRkLTUxZDctNGZjZi1hNTc1LWViZjQ5MmZjZDg2NCIsImp0aSI6ImJlNDBkNmVlLWU2YzItNGYwMi1iNjA2LTkzZmJiNDcyMGZiZSIsImlhdCI6IjE2ODI1NzgzNjA0NjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJzYXNoYXRrYWNodWswOUB1a3IubmV0IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoidXNlciIsIm5iZiI6MTY4MjU3ODM2MCwiZXhwIjoxNjgyNTgxOTYwLCJpc3MiOiJqb2Itd3JhcHBlciJ9.stzw2XmIHHP0vPhbEaccSf04O1eE5tP9F6hYEcJ9Ee4'
        }
    }
    );
  sleep(1);
}
