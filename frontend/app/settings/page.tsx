export default function SettingsPage(): React.JSX.Element {
    return (
        <div>
            Settings
        </div>
    );
}

//
//
// function DailyTrackerGrid(): React.JSX.Element {
//
//     return (
//         <div>
//             {
//                 /* Show Github Contributions Grid for the last 365 days */
//             }
//
//             {
//                 for (let i = 0; i < 365; i++) {
//                 <DailyTrackerCell />
//             }
//             }
//         </div >
//     );
// }
//
// function DailyTrackerCell(): React.JSX.Element {
//     return (
//         <div>
//             <div className="cell"></div>
//         </div>
//     );
// }
//
//
// const generateGridData = (contributions) => {
//     const daysInYear = 365;
//     const today = new Date();
//     const startDate = new Date(today.setDate(today.getDate() - daysInYear));
//
//     const grid = Array.from({ length: daysInYear }, (_, i) => {
//         const date = new Date(startDate);
//         date.setDate(startDate.getDate() + i);
//
//         return {
//             date: date.toISOString().split("T")[0],
//             contributed: contributions.some(
//                 (c) => c.contribution_date === date.toISOString().split("T")[0]
//             ),
//         };
//     });
//
//     return grid;
// };
