import { createSlice } from "@reduxjs/toolkit";

const authSlice = createSlice({
  name: "auth",
  initialState: {
    username: null,
    role: null,
    employeeId: null,
  },
  reducers: {
    login: (state, action) => {
      console.log(action.payload);
      state.username = action.payload.username;
      state.role = action.payload.role;
      state.employeeId = action.payload.employeeId;
    },
    logout: (state) => {
      state.username = null;
      state.role = null;
      state.employeeId = null;
    },
  },
});

export const { login, logout } = authSlice.actions;
export default authSlice.reducer;