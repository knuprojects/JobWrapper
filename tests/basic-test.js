import http from 'k6/http';
import { sleep } from 'k6';
import * as config from './config.js';

export default function () {
  http.get(config.API_VACANCIES);
  sleep(1);
}
