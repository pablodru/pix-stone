import http from "k6/http";
import { sleep } from "k6";
import { SharedArray } from "k6/data";

export const options = {
	vus: 30,
	duration: "60s",
};

export default function (){

    const paymentToCreate = {
        origin:{
            user:{
                cpf: "42840822464"
            },
            account: {
                number: "626323358",
                agency: "8817"
            }
        },
        destiny: {
            key: {
                value: "35138917407",
                type: "Phone"
            }
        },
        amount: generateRandomNumber(1000, 100000)
    }
    const body = JSON.stringify(paymentToCreate);
    const headers = { "Content-Type": "application/json", "Authorization": `Bearer 59252908-041b-4852-b740-f005e7dc4bb4` };

    http.post(`http://localhost:5109/payments`, body, { headers });

    sleep(1);
}

function generateRandomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}