import http from "k6/http";
import { sleep } from "k6";
import { SharedArray } from "k6/data";

export const options = {
	vus: 30,
	duration: "30s",
};

export default function (){

    const paymentToCreate = {
        origin:{
            user:{
                cpf: "59410089172"
            },
            account: {
                number: "117996841",
                agency: "3160"
            }
        },
        destiny: {
            key: {
                value: "99440383743",
                type: "Phone"
            }
        },
        amount: generateRandomNumber(1000, 100000)
    }
    const body = JSON.stringify(paymentToCreate);
    const headers = { "Content-Type": "application/json", "Authorization": `Bearer 7d5b6b3e-b2e1-4d7e-8c8a-b246ec0b8077` };

    http.post(`http://localhost:5109/payments`, body, { headers });

    sleep(1);
}

function generateRandomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}