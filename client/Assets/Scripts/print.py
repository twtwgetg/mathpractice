# 打印所有的文件到PDF
import os
from reportlab.pdfgen import canvas
from reportlab.lib.pagesizes import A4
from reportlab.lib.units import cm
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont

def print_all_files_to_pdf(file_dir):
    pdf_file = canvas.Canvas("all_files.pdf", pagesize=A4)
    page_number = 1
    
    # Register Chinese font
    # Option 1: Use system font (Windows)
    try:
        pdfmetrics.registerFont(TTFont('SimSun', 'C:/Windows/Fonts/simsun.ttc'))
        pdf_file.setFont("SimSun", 12)
    except:
        # Fallback to default if Chinese font not available
        pdf_file.setFont("Helvetica", 12)
    
    line_height = 0.5 * cm
    # Increased top margin (from 29cm to 27cm to give more space)
    margin_top = 27 * cm
    margin_left = 1 * cm
    # Bottom margin for page numbers
    margin_bottom = 1 * cm
    
    def draw_page_number():
        pdf_file.setFont("SimSun", 10)
        pdf_file.drawRightString(A4[0] - margin_left, margin_bottom, f"第 {page_number} 页")
        # Reset to main font for content
        try:
            pdf_file.setFont("SimSun", 12)
        except:
            pdf_file.setFont("Helvetica", 12)
    
    for root, dirs, files in os.walk(file_dir):
        for file in files:
            if file.endswith(".cs"):
                file_path = os.path.join(root, file)
                try:
                    with open(file_path, "r", encoding='utf-8') as f:
                        lines = f.readlines()
                        y_position = margin_top
                        
                        # Draw file path header
                        if y_position < 3 * cm:
                            draw_page_number()
                            pdf_file.showPage()
                            page_number += 1
                            y_position = margin_top
                        
                        pdf_file.drawString(margin_left, y_position, f"文件: {file_path}")
                        y_position -= line_height * 2  # Extra space after header
                        
                        for line in lines:
                            # Handle text that's too long for the page
                            if y_position < 2 * cm:
                                draw_page_number()
                                pdf_file.showPage()
                                page_number += 1
                                y_position = margin_top
                                # Redraw file header on new page
                                pdf_file.drawString(margin_left, y_position, f"文件: {file_path}")
                                y_position -= line_height * 2  # Extra space after header
                            
                            pdf_file.drawString(margin_left, y_position, line.strip())
                            # Move down by 2 lines (1 for text, 1 for blank space)
                            y_position -= line_height * 2
                            
                        # Add extra space between files (equivalent to 2 blank lines)
                        y_position -= line_height * 2
                        # Show page if we have content
                        if y_position < margin_top:  # Only if we actually drew something
                            draw_page_number()
                            pdf_file.showPage()
                            page_number += 1
                except Exception as e:
                    print(f"Error reading file {file_path}: {e}")
    
    # Draw page number on the last page if it has content
    draw_page_number()
    pdf_file.save()

# Usage
print_all_files_to_pdf(r"c:\Users\Administrator\mathpractice\client\Assets\Scripts")