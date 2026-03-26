let currentView = 'week';

function switchView(view) {
    currentView = view;
    
    // Update UI buttons
    ['day', 'week', 'month'].forEach(v => {
        const btn = document.getElementById(`view-${v}`);
        if (v === view) {
            btn.classList.add('bg-white', 'shadow-sm', 'text-capy-primary');
            btn.classList.remove('text-slate-500');
        } else {
            btn.classList.remove('bg-white', 'shadow-sm', 'text-capy-primary');
            btn.classList.add('text-slate-500');
        }
    });

    renderCalendar();
}

function renderCalendar() {
    const grid = document.getElementById('calendar-grid');
    const loader = document.getElementById('calendar-loader');
    
    loader.classList.remove('opacity-0', 'pointer-events-none');
    
    setTimeout(() => {
        if (currentView === 'day') {
            renderDayView(grid);
        } else if (currentView === 'week') {
            renderWeekView(grid);
        } else if (currentView === 'month') {
            renderMonthView(grid);
        }
        loader.classList.add('opacity-0', 'pointer-events-none');
    }, 500);
}

function renderDayView(container) {
    let html = `
        <div class="grid grid-cols-[100px_1fr] h-full">
            <div class="border-r border-slate-100 py-4">
                ${Array.from({length: 18}, (_, i) => i + 6).map(hour => `
                    <div class="h-20 text-[10px] font-extrabold text-slate-400 text-right pr-4">${hour}:00</div>
                `).join('')}
            </div>
            <div class="relative py-4">
                ${Array.from({length: 18}, (_, i) => i + 6).map(hour => `
                    <div class="h-20 border-b border-slate-50 w-full relative group">
                        <div class="absolute inset-x-0 h-px bg-slate-100 transition-colors group-hover:bg-capy-primary/20"></div>
                    </div>
                `).join('')}
                <!-- Sample Task -->
                <div class="absolute top-[200px] left-4 right-8 h-40 bg-capy-primary/10 border-l-4 border-capy-primary rounded-xl p-4 shadow-sm hover:shadow-md transition-all cursor-pointer">
                    <p class="text-xs font-extrabold text-capy-primary uppercase tracking-widest mb-1">Deep Learning</p>
                    <p class="font-bold text-slate-800">Module 3: Neural Networks</p>
                    <p class="text-[10px] text-slate-500 font-medium">9:00 AM - 11:30 AM</p>
                </div>
            </div>
        </div>
    `;
    container.innerHTML = html;
}

function renderWeekView(container) {
    const days = ['Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7', 'CN'];
    let html = `
        <div class="grid grid-cols-[60px_repeat(7,1fr)] h-full overflow-hidden">
            <div class="border-r border-slate-100 pt-12">
                ${Array.from({length: 18}, (_, i) => i + 6).map(hour => `
                    <div class="h-16 text-[9px] font-extrabold text-slate-400 text-center">${hour}:00</div>
                `).join('')}
            </div>
            ${days.map(day => `
                <div class="border-l border-slate-50">
                    <div class="h-12 border-b border-slate-100 flex items-center justify-center text-[11px] font-extrabold text-slate-500 uppercase tracking-wider bg-slate-50/50">${day}</div>
                    <div class="relative">
                        ${Array.from({length: 18}).map(() => `
                            <div class="h-16 border-b border-slate-50/50 w-full"></div>
                        `).join('')}
                    </div>
                </div>
            `).join('')}
        </div>
    `;
    container.innerHTML = html;
}

function renderMonthView(container) {
    let html = `
        <div class="grid grid-cols-7 h-full border-t border-l border-slate-100">
            ${['T2', 'T3', 'T4', 'T5', 'T6', 'T7', 'CN'].map(d => `
                <div class="h-12 flex items-center justify-center text-[10px] font-extrabold text-slate-400 border-r border-b border-slate-100 uppercase bg-slate-50/50">${d}</div>
            `).join('')}
            ${Array.from({length: 31}, (_, i) => i + 1).map(day => {
                const isBusy = day % 3 === 0;
                const isVeryBusy = day % 7 === 0;
                return `
                    <div class="h-32 p-3 border-r border-b border-slate-100 hover:bg-slate-50 transition-colors group cursor-pointer relative">
                        <span class="text-sm font-bold text-slate-400 group-hover:text-capy-primary transition-colors">${day}</span>
                        ${isBusy ? `<div class="absolute bottom-3 left-3 flex gap-1">
                            <span class="w-1.5 h-1.5 rounded-full ${isVeryBusy ? 'bg-red-400' : 'bg-green-400'}"></span>
                            ${day % 5 === 0 ? '<span class="w-1.5 h-1.5 rounded-full bg-amber-400"></span>' : ''}
                        </div>` : ''}
                    </div>
                `;
            }).join('')}
        </div>
    `;
    container.innerHTML = html;
}

function openAIAdvice() {
    const modal = document.getElementById('ai-modal');
    const content = document.getElementById('ai-modal-content');
    modal.classList.remove('hidden');
    setTimeout(() => {
        modal.classList.remove('opacity-0');
        content.classList.remove('scale-95');
    }, 10);
}

function closeAIAdvice() {
    const modal = document.getElementById('ai-modal');
    const content = document.getElementById('ai-modal-content');
    modal.classList.add('opacity-0');
    content.classList.add('scale-95');
    setTimeout(() => {
        modal.classList.add('hidden');
    }, 300);
}

// Initial render
document.addEventListener('DOMContentLoaded', () => {
    renderCalendar();
});
