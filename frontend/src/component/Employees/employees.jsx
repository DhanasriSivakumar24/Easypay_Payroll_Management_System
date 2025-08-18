import { useEffect, useState } from "react"
import { SearchEmployees } from "../../service/employee.service"

const Employees =()=>{
    const [employees,setEmployees]=useState([]);
    const searchObject ={
        "firstName": "asha",
        "email": "asha",
        "departments": [
            1,2,3,4
        ],
        "statusId": 1,
        "pageNumber": 1,
        "pageSize": 2,
        "sort": 1
}
useEffect(()=>{
    SearchEmployees(searchObject)
    .then((result)=>{
        setEmployees(result.data.employees??[])
    })
    .catch((err)=>{
        console.error(err)
    })
},[])
return(<>
{
    employees.length ===0 && (<div>No result</div>)
}
{
    employees.map((employee)=><section key={employee.id}>
        {employee.firstName}
    </section>)
}
</>)}

export default Employees;